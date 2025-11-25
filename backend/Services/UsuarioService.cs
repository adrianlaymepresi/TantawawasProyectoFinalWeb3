using backend.Data;
using backend.Models;
using backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace backend.Services
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> ObtenerPorIdAsync(UsuarioIdDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.Id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            return usuario;
        }

        public async Task<List<Usuario>> BuscarPorCarnetIdentidadAsync(UsuarioBuscarPorCI dto)
        {
            string ci = dto.CarnetIdentidad.Trim();

            var usuarios = await _context.Usuarios
                .Where(u => u.CarnetIdentidad.ToString().Contains(ci))
                .ToListAsync();

            if (!usuarios.Any())
                throw new Exception("No se encontraron usuarios con el CI proporcionado");

            return usuarios;
        }

        public async Task<List<Usuario>> BuscarPorNombreCompletoAsync(UsuarioBuscarPorNombreCompleto dto)
        {
            string nombre = dto.NombreCompleto.Trim().ToLower();

            var usuarios = await _context.Usuarios
                .Where(u =>
                    (u.Nombres + " " + u.Apellidos).ToLower().Contains(nombre))
                .ToListAsync();

            if (!usuarios.Any())
                throw new Exception("No se encontraron usuarios con ese nombre");

            return usuarios;
        }

        public async Task<Usuario> CrearAsync(UsuarioCrearDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("El email ya está registrado");

            if (await _context.Usuarios.AnyAsync(u => u.CarnetIdentidad == dto.CarnetIdentidad))
                throw new Exception("El carnet de identidad ya está registrado");

            var rolExiste = await _context.Roles.AnyAsync(r => r.Id == dto.RolId);
            if (!rolExiste)
                throw new Exception("El rol especificado no existe");

            var usuario = new Usuario
            {
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                CarnetIdentidad = dto.CarnetIdentidad,
                Email = dto.Email,
                RolId = dto.RolId,
                Password = HashPassword(dto.Password),
                EsUsuarioActivo = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> ActualizarAsync(UsuarioActualizarDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.Id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            if (await _context.Usuarios.AnyAsync(u => u.CarnetIdentidad == dto.CarnetIdentidad && u.Id != dto.Id))
                throw new Exception("El carnet de identidad ya está registrado por otro usuario");

            var rolExiste = await _context.Roles.AnyAsync(r => r.Id == dto.RolId);
            if (!rolExiste)
                throw new Exception("El rol especificado no existe");

            usuario.Nombres = dto.Nombres;
            usuario.Apellidos = dto.Apellidos;
            usuario.CarnetIdentidad = dto.CarnetIdentidad;
            usuario.Email = dto.Email;
            usuario.RolId = dto.RolId;

            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> ActualizarPasswordAsync(UsuarioActualizarPasswordDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.Id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            usuario.Password = HashPassword(dto.NuevoPassword);

            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> EliminarLogicoAsync(UsuarioIdDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.Id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            usuario.EsUsuarioActivo = false;
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> EliminarFisicoAsync(UsuarioIdDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.Id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        private string HashPassword(string password)
        {
            var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
