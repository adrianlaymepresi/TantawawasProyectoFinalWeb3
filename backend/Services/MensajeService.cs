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

        public async Task<List<MensajeObtenerDto>> ObtenerTodosAsync()
        {
            return await _context.Mensajes
                .Include(m => m.Curso)
                .Include(m => m.Usuario)
                .OrderByDescending(m => m.FechaEnvio)
                .Select(m => new MensajeObtenerDto
                {
                    Id = m.Id,
                    Contenido = m.Contenido,
                    ArchivoAdjunto = m.ArchivoAdjunto,
                    FechaEnvio = m.FechaEnvio,
                    CursoId = m.CursoId,
                    CursoNombre = m.Curso != null ? m.Curso.Nombre : "Curso no identificado",
                    UsuarioId = m.UsuarioId,
                    UsuarioNombreCompleto = m.Usuario != null ? m.Usuario.Nombres + " " + m.Usuario.Apellidos : "Usuario no identificado"
                })
                .ToListAsync();
        }

        public async Task<List<MensajeSimpleDto>> ObtenerPorCursoAsync(int cursoId)
        {
            return await _context.Mensajes
                .Include(m => m.Usuario)
                .Where(m => m.CursoId == cursoId)
                .OrderByDescending(m => m.FechaEnvio)
                .Select(m => new MensajeSimpleDto
                {
                    Id = m.Id,
                    Contenido = m.Contenido,
                    FechaEnvio = m.FechaEnvio,
                    UsuarioId = m.UsuarioId,
                    UsuarioNombreCompleto = m.Usuario != null ? m.Usuario.Nombres + " " + m.Usuario.Apellidos : "Usuario no identificado"
                })
                .ToListAsync();
        }

        public async Task<List<MensajePorUsuarioDto>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _context.Mensajes
                .Include(m => m.Curso)
                .Where(m => m.UsuarioId == usuarioId)
                .OrderByDescending(m => m.FechaEnvio)
                .Select(m => new MensajePorUsuarioDto
                {
                    Id = m.Id,
                    Contenido = m.Contenido,
                    FechaEnvio = m.FechaEnvio,
                    CursoId = m.CursoId,
                    CursoNombre = m.Curso != null ? m.Curso.Nombre : "Curso no identificado"
                })
                .ToListAsync();
        }

        public async Task<MensajeObtenerDto> ObtenerPorIdAsync(int id)
        {
            var mensaje = await _context.Mensajes
                .Include(m => m.Curso)
                .Include(m => m.Usuario)
                .Where(m => m.Id == id)
                .Select(m => new MensajeObtenerDto
                {
                    Id = m.Id,
                    Contenido = m.Contenido,
                    ArchivoAdjunto = m.ArchivoAdjunto,
                    FechaEnvio = m.FechaEnvio,
                    CursoId = m.CursoId,
                    CursoNombre = m.Curso != null ? m.Curso.Nombre : "Curso no identificado",
                    UsuarioId = m.UsuarioId,
                    UsuarioNombreCompleto = m.Usuario != null ? m.Usuario.Nombres + " " + m.Usuario.Apellidos : "Usuario no identificado"
                })
                .FirstOrDefaultAsync();

            if (mensaje == null)
                throw new Exception("Mensaje no encontrado");

            return mensaje;
        }

        public async Task<MensajeObtenerDto> CrearAsync(MensajeCrearDto dto)
        {
            var curso = await _context.Cursos.FindAsync(dto.CursoId);
            if (curso == null)
                throw new Exception("El curso especificado no existe");

            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);
            if (usuario == null)
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

            return new MensajeObtenerDto
            {
                Id = mensaje.Id,
                Contenido = mensaje.Contenido,
                ArchivoAdjunto = mensaje.ArchivoAdjunto,
                FechaEnvio = mensaje.FechaEnvio,
                CursoId = mensaje.CursoId,
                CursoNombre = curso.Nombre,
                UsuarioId = mensaje.UsuarioId,
                UsuarioNombreCompleto = usuario.Nombres + " " + usuario.Apellidos
            };
        }

        public async Task<MensajeObtenerDto> ActualizarAsync(MensajeActualizarDto dto)
        {
            var mensaje = await _context.Mensajes
                .Include(m => m.Curso)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.Id == dto.Id);
            
            if (mensaje == null)
                throw new Exception("Mensaje no encontrado");

            if (mensaje.Curso == null)
                throw new Exception("El curso asignado al mensaje ya no existe");

            if (mensaje.Usuario == null)
                throw new Exception("El usuario asignado al mensaje ya no existe");

            if (!string.IsNullOrEmpty(dto.Contenido))
                mensaje.Contenido = dto.Contenido;

            if (dto.ArchivoAdjunto != null)
                mensaje.ArchivoAdjunto = dto.ArchivoAdjunto;

            await _context.SaveChangesAsync();
            
            return new MensajeObtenerDto
            {
                Id = mensaje.Id,
                Contenido = mensaje.Contenido,
                ArchivoAdjunto = mensaje.ArchivoAdjunto,
                FechaEnvio = mensaje.FechaEnvio,
                CursoId = mensaje.CursoId,
                CursoNombre = mensaje.Curso.Nombre,
                UsuarioId = mensaje.UsuarioId,
                UsuarioNombreCompleto = mensaje.Usuario.Nombres + " " + mensaje.Usuario.Apellidos
            };
        }

        public async Task<MensajeObtenerDto> EliminarFisicoAsync(int id)
        {
            var mensaje = await _context.Mensajes
                .Include(m => m.Curso)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (mensaje == null)
                throw new Exception("Mensaje no encontrado");

            var mensajeDto = new MensajeObtenerDto
            {
                Id = mensaje.Id,
                Contenido = mensaje.Contenido,
                ArchivoAdjunto = mensaje.ArchivoAdjunto,
                FechaEnvio = mensaje.FechaEnvio,
                CursoId = mensaje.CursoId,
                CursoNombre = mensaje.Curso != null ? mensaje.Curso.Nombre : "Curso no identificado",
                UsuarioId = mensaje.UsuarioId,
                UsuarioNombreCompleto = mensaje.Usuario != null ? mensaje.Usuario.Nombres + " " + mensaje.Usuario.Apellidos : "Usuario no identificado"
            };

            _context.Mensajes.Remove(mensaje);
            await _context.SaveChangesAsync();
            
            return mensajeDto;
        }
    }
}
