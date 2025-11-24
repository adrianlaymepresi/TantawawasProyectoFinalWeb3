using backend.Data;
using backend.Models;
using backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class RolService
    {
        private readonly AppDbContext _context;

        public RolService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Rol>> ObtenerTodosAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Rol> ObtenerPorIdAsync(RolIdDto dto)
        {
            var rol = await _context.Roles.FindAsync(dto.Id);
            if (rol == null)
                throw new Exception("Rol no encontrado");

            return rol;
        }

        public async Task<Rol> CrearAsync(RolCrearDto dto)
        {
            if (await _context.Roles.AnyAsync(r => r.NombreRol == dto.NombreRol))
                throw new Exception("El rol ya existe");

            var rol = new Rol
            {
                NombreRol = dto.NombreRol
            };

            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();
            return rol;
        }

        public async Task<Rol> ActualizarAsync(RolActualizarDto dto)
        {
            var rol = await _context.Roles.FindAsync(dto.Id);
            if (rol == null)
                throw new Exception("Rol no encontrado");

            rol.NombreRol = dto.NombreRol;

            await _context.SaveChangesAsync();
            return rol;
        }

        public async Task<Rol> EliminarFisicoAsync(RolIdDto dto)
        {
            var rol = await _context.Roles.FindAsync(dto.Id);
            if (rol == null)
                throw new Exception("Rol no encontrado");

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();
            return rol;
        }
    }
}
