using backend.Data;
using backend.Models;
using backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class InscripcionService
    {
        private readonly AppDbContext _context;

        public InscripcionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<InscripcionObtenerDto>> ObtenerTodasAsync()
        {
            return await _context.Inscripciones
                .Include(i => i.Estudiante)
                .Include(i => i.Curso)
                .Select(i => new InscripcionObtenerDto
                {
                    Id = i.Id,
                    EstudianteId = i.EstudianteId,
                    NombresEstudiante = i.Estudiante.Nombres,
                    ApellidosEstudiante = i.Estudiante.Apellidos,
                    CursoId = i.CursoId,
                    NombreCurso = i.Curso.Nombre,
                    FechaInscripcion = i.FechaInscripcion.ToString("yyyy-MM-dd HH:mm:ss")
                })
                .ToListAsync();
        }

        public async Task<InscripcionObtenerDto> ObtenerPorIdAsync(InscripcionIdDto dto)
        {
            var inscripcion = await _context.Inscripciones
                .Include(i => i.Estudiante)
                .Include(i => i.Curso)
                .FirstOrDefaultAsync(i => i.Id == dto.Id);

            if (inscripcion == null)
                throw new Exception("Inscripción no encontrada");

            return new InscripcionObtenerDto
            {
                Id = inscripcion.Id,
                EstudianteId = inscripcion.EstudianteId,
                NombresEstudiante = inscripcion.Estudiante.Nombres,
                ApellidosEstudiante = inscripcion.Estudiante.Apellidos,
                CursoId = inscripcion.CursoId,
                NombreCurso = inscripcion.Curso.Nombre,
                FechaInscripcion = inscripcion.FechaInscripcion.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }

        public async Task<List<InscripcionObtenerDto>> BuscarPorEstudianteAsync(InscripcionBuscarPorEstudianteDto dto)
        {
            var existe = await _context.Usuarios.AnyAsync(u => u.Id == dto.EstudianteId);
            if (!existe)
                throw new Exception("El estudiante no existe");

            var inscripciones = await _context.Inscripciones
                .Include(i => i.Curso)
                .Include(i => i.Estudiante)
                .Where(i => i.EstudianteId == dto.EstudianteId)
                .ToListAsync();

            if (!inscripciones.Any())
                throw new Exception("El estudiante no está inscrito en ningún curso");

            return inscripciones.Select(i => new InscripcionObtenerDto
            {
                Id = i.Id,
                EstudianteId = i.EstudianteId,
                NombresEstudiante = i.Estudiante.Nombres,
                ApellidosEstudiante = i.Estudiante.Apellidos,
                CursoId = i.CursoId,
                NombreCurso = i.Curso.Nombre,
                FechaInscripcion = i.FechaInscripcion.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();
        }

        public async Task<List<InscripcionObtenerDto>> BuscarPorCursoAsync(InscripcionBuscarPorCursoDto dto)
        {
            var existe = await _context.Cursos.AnyAsync(c => c.Id == dto.CursoId);
            if (!existe)
                throw new Exception("El curso no existe");

            var inscripciones = await _context.Inscripciones
                .Include(i => i.Curso)
                .Include(i => i.Estudiante)
                .Where(i => i.CursoId == dto.CursoId)
                .ToListAsync();

            if (!inscripciones.Any())
                throw new Exception("El curso no tiene estudiantes inscritos");

            return inscripciones.Select(i => new InscripcionObtenerDto
            {
                Id = i.Id,
                EstudianteId = i.EstudianteId,
                NombresEstudiante = i.Estudiante.Nombres,
                ApellidosEstudiante = i.Estudiante.Apellidos,
                CursoId = i.CursoId,
                NombreCurso = i.Curso.Nombre,
                FechaInscripcion = i.FechaInscripcion.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();
        }

        public async Task<InscripcionObtenerDto> CrearAsync(InscripcionCrearDto dto)
        {
            var estudiante = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == dto.EstudianteId);

            if (estudiante == null)
                throw new Exception("El estudiante no existe");

            if (estudiante.Rol == null || estudiante.Rol.NombreRol != "Estudiante")
                throw new Exception("El usuario no tiene el rol de Estudiante");

            var curso = await _context.Cursos.FirstOrDefaultAsync(c => c.Id == dto.CursoId);
            if (curso == null)
                throw new Exception("El curso no existe");

            var duplicado = await _context.Inscripciones.AnyAsync(i =>
                i.EstudianteId == dto.EstudianteId && i.CursoId == dto.CursoId
            );

            if (duplicado)
                throw new Exception("Este estudiante ya está inscrito en este curso");

            var inscripcion = new Inscripcion
            {
                EstudianteId = dto.EstudianteId,
                CursoId = dto.CursoId,
                FechaInscripcion = DateTime.Now
            };

            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync();

            return new InscripcionObtenerDto
            {
                Id = inscripcion.Id,
                EstudianteId = estudiante.Id,
                NombresEstudiante = estudiante.Nombres,
                ApellidosEstudiante = estudiante.Apellidos,
                CursoId = curso.Id,
                NombreCurso = curso.Nombre,
                FechaInscripcion = inscripcion.FechaInscripcion.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }

        public async Task<InscripcionObtenerDto> ActualizarAsync(InscripcionActualizarDto dto)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(dto.Id);
            if (inscripcion == null)
                throw new Exception("Inscripción no encontrada");

            var estudiante = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == dto.EstudianteId);

            if (estudiante == null)
                throw new Exception("El estudiante no existe");

            if (estudiante.Rol == null || estudiante.Rol.NombreRol != "Estudiante")
                throw new Exception("El usuario no tiene el rol de Estudiante");

            var curso = await _context.Cursos.FirstOrDefaultAsync(c => c.Id == dto.CursoId);
            if (curso == null)
                throw new Exception("El curso no existe");

            var duplicado = await _context.Inscripciones.AnyAsync(i =>
                i.EstudianteId == dto.EstudianteId &&
                i.CursoId == dto.CursoId &&
                i.Id != dto.Id
            );

            if (duplicado)
                throw new Exception("Ya existe otra inscripción igual");

            inscripcion.EstudianteId = dto.EstudianteId;
            inscripcion.CursoId = dto.CursoId;

            await _context.SaveChangesAsync();

            return new InscripcionObtenerDto
            {
                Id = inscripcion.Id,
                EstudianteId = estudiante.Id,
                NombresEstudiante = estudiante.Nombres,
                ApellidosEstudiante = estudiante.Apellidos,
                CursoId = curso.Id,
                NombreCurso = curso.Nombre,
                FechaInscripcion = inscripcion.FechaInscripcion.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }

        public async Task<InscripcionObtenerDto> EliminarFisicoAsync(InscripcionIdDto dto)
        {
            var inscripcion = await _context.Inscripciones
                .Include(i => i.Estudiante)
                .Include(i => i.Curso)
                .FirstOrDefaultAsync(i => i.Id == dto.Id);

            if (inscripcion == null)
                throw new Exception("Inscripción no encontrada");

            _context.Inscripciones.Remove(inscripcion);
            await _context.SaveChangesAsync();

            return new InscripcionObtenerDto
            {
                Id = inscripcion.Id,
                EstudianteId = inscripcion.EstudianteId,
                NombresEstudiante = inscripcion.Estudiante.Nombres,
                ApellidosEstudiante = inscripcion.Estudiante.Apellidos,
                CursoId = inscripcion.CursoId,
                NombreCurso = inscripcion.Curso.Nombre,
                FechaInscripcion = inscripcion.FechaInscripcion.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }
    }
}
