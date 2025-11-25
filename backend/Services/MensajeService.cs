using backend.Data;
using backend.Models;
using backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class MensajeService
    {
        private readonly AppDbContext _context;

        public MensajeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Mensaje>> ObtenerTodosAsync()
        {
            return await _context.Mensajes
                .Include(m => m.Curso)
                .Include(m => m.Usuario)
                .OrderByDescending(m => m.FechaEnvio)
                .ToListAsync();
        }

        public async Task<List<Mensaje>> ObtenerPorCursoAsync(int cursoId)
        {
            return await _context.Mensajes
                .Where(m => m.CursoId == cursoId)
                .Include(m => m.Curso)
                .Include(m => m.Usuario)
                .OrderByDescending(m => m.FechaEnvio)
                .ToListAsync();
        }

        public async Task<List<Mensaje>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _context.Mensajes
                .Where(m => m.UsuarioId == usuarioId)
                .Include(m => m.Curso)
                .Include(m => m.Usuario)
                .OrderByDescending(m => m.FechaEnvio)
                .ToListAsync();
        }

        public async Task<Mensaje> ObtenerPorIdAsync(MensajeIdDto dto)
        {
            var mensaje = await _context.Mensajes
                .Include(m => m.Curso)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.Id == dto.Id);

            if (mensaje == null)
                throw new Exception("Mensaje no encontrado");

            return mensaje;
        }

        public async Task<Mensaje> CrearAsync(MensajeCrearDto dto)
        {
            var cursoExiste = await _context.Cursos.AnyAsync(c => c.Id == dto.CursoId);
            if (!cursoExiste)
                throw new Exception("El curso especificado no existe");

            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == dto.UsuarioId);
            if (!usuarioExiste)
                throw new Exception("El usuario especificado no existe");

            var mensaje = new Mensaje
            {
                Contenido = dto.Contenido,
                ArchivoAdjunto = dto.ArchivoAdjunto,
                FechaEnvio = dto.FechaEnvio,
                CursoId = dto.CursoId,
                UsuarioId = dto.UsuarioId
            };

            _context.Mensajes.Add(mensaje);
            await _context.SaveChangesAsync();

            return mensaje;
        }

        public async Task<Mensaje> ActualizarAsync(MensajeActualizarDto dto)
        {
            var mensaje = await _context.Mensajes.FindAsync(dto.Id);
            if (mensaje == null)
                throw new Exception("Mensaje no encontrado");

            if (!string.IsNullOrEmpty(dto.Contenido))
                mensaje.Contenido = dto.Contenido;

            if (dto.ArchivoAdjunto != null)
                mensaje.ArchivoAdjunto = dto.ArchivoAdjunto;

            await _context.SaveChangesAsync();

            return mensaje;
        }

        public async Task<Mensaje> EliminarFisicoAsync(MensajeIdDto dto)
        {
            var mensaje = await _context.Mensajes.FindAsync(dto.Id);
            if (mensaje == null)
                throw new Exception("Mensaje no encontrado");

            _context.Mensajes.Remove(mensaje);
            await _context.SaveChangesAsync();
            return mensaje;
        }
    }
}
