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

        public async Task<List<Inscripcion>> ObtenerTodasAsync()
        {
            return await _context.Inscripciones
                .Include(i => i.Estudiante)
                .Include(i => i.Curso)
                .ToListAsync();
        }

        public async Task<Inscripcion> ObtenerPorIdAsync(InscripcionIdDto dto)
        {
            var inscripcion = await _context.Inscripciones
                .Include(i => i.Estudiante)
                .Include(i => i.Curso)
                .FirstOrDefaultAsync(i => i.Id == dto.Id);

            if (inscripcion == null)
                throw new Exception("Inscripción no encontrada");

            return inscripcion;
        }

        public async Task<List<Inscripcion>> BuscarPorEstudianteAsync(InscripcionBuscarPorEstudianteDto dto)
        {
            var existe = await _context.Usuarios.AnyAsync(u => u.Id == dto.EstudianteId);
            if (!existe)
                throw new Exception("El estudiante no existe");

            var inscripciones = await _context.Inscripciones
                .Where(i => i.EstudianteId == dto.EstudianteId)
                .Include(i => i.Curso)
                .Include(i => i.Estudiante)
                .ToListAsync();

            if (!inscripciones.Any())
                throw new Exception("El estudiante no está inscrito en ningún curso");

            return inscripciones;
        }

        public async Task<List<Inscripcion>> BuscarPorCursoAsync(InscripcionBuscarPorCursoDto dto)
        {
            var existe = await _context.Cursos.AnyAsync(c => c.Id == dto.CursoId);
            if (!existe)
                throw new Exception("El curso no existe");

            var inscripciones = await _context.Inscripciones
                .Where(i => i.CursoId == dto.CursoId)
                .Include(i => i.Estudiante)
                .Include(i => i.Curso)
                .ToListAsync();

            if (!inscripciones.Any())
                throw new Exception("El curso no tiene estudiantes inscritos");

            return inscripciones;
        }

        public async Task<Inscripcion> CrearAsync(InscripcionCrearDto dto)
        {
            var estudianteExiste = await _context.Usuarios.AnyAsync(u => u.Id == dto.EstudianteId);
            if (!estudianteExiste)
                throw new Exception("El estudiante no existe");

            var cursoExiste = await _context.Cursos.AnyAsync(c => c.Id == dto.CursoId);
            if (!cursoExiste)
                throw new Exception("El curso no existe");

            var duplicado = await _context.Inscripciones.AnyAsync(i =>
                i.EstudianteId == dto.EstudianteId &&
                i.CursoId == dto.CursoId
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
            return inscripcion;
        }

        public async Task<Inscripcion> ActualizarAsync(InscripcionActualizarDto dto)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(dto.Id);
            if (inscripcion == null)
                throw new Exception("Inscripción no encontrada");

            var estudianteExiste = await _context.Usuarios.AnyAsync(u => u.Id == dto.EstudianteId);
            if (!estudianteExiste)
                throw new Exception("El estudiante no existe");

            var cursoExiste = await _context.Cursos.AnyAsync(c => c.Id == dto.CursoId);
            if (!cursoExiste)
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
            return inscripcion;
        }

        public async Task<Inscripcion> EliminarFisicoAsync(InscripcionIdDto dto)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(dto.Id);
            if (inscripcion == null)
                throw new Exception("Inscripción no encontrada");

            _context.Inscripciones.Remove(inscripcion);
            await _context.SaveChangesAsync();
            return inscripcion;
        }
    }
}
