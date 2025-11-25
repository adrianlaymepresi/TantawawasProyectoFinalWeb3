using backend.Data;
using backend.Models;
using backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class EvaluacionService
    {
        private readonly AppDbContext _context;

        public EvaluacionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Evaluacion>> ObtenerTodosAsync()
        {
            return await _context.Evaluaciones
                .Include(e => e.Curso)
                .Include(e => e.Resultados)
                .OrderByDescending(e => e.FechaCreacion)
                .ToListAsync();
        }

        public async Task<List<Evaluacion>> ObtenerPorCursoAsync(int cursoId)
        {
            return await _context.Evaluaciones
                .Where(e => e.CursoId == cursoId)
                .Include(e => e.Curso)
                .Include(e => e.Resultados)
                .OrderByDescending(e => e.FechaCreacion)
                .ToListAsync();
        }

        public async Task<Evaluacion> ObtenerPorIdAsync(EvaluacionIdDto dto)
        {
            var evaluacion = await _context.Evaluaciones
                .Include(e => e.Curso)
                .Include(e => e.Resultados)
                .FirstOrDefaultAsync(e => e.Id == dto.Id);

            if (evaluacion == null)
                throw new Exception("Evaluación no encontrada");

            return evaluacion;
        }

        public async Task<List<Evaluacion>> BuscarPorTituloAsync(string titulo)
        {
            var evaluaciones = await _context.Evaluaciones
                .Where(e => e.Titulo.ToLower().Contains(titulo.Trim().ToLower()))
                .Include(e => e.Curso)
                .Include(e => e.Resultados)
                .OrderByDescending(e => e.FechaCreacion)
                .ToListAsync();

            if (!evaluaciones.Any())
                throw new Exception("No se encontraron evaluaciones con ese título");

            return evaluaciones;
        }

        public async Task<Evaluacion> CrearAsync(EvaluacionCrearDto dto)
        {
            var cursoExiste = await _context.Cursos.AnyAsync(c => c.Id == dto.CursoId);
            if (!cursoExiste)
                throw new Exception("El curso especificado no existe");

            var evaluacion = new Evaluacion
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                FechaCreacion = dto.FechaCreacion,
                CursoId = dto.CursoId
            };

            _context.Evaluaciones.Add(evaluacion);
            await _context.SaveChangesAsync();

            return evaluacion;
        }

        public async Task<Evaluacion> ActualizarAsync(EvaluacionActualizarDto dto)
        {
            var evaluacion = await _context.Evaluaciones.FindAsync(dto.Id);
            if (evaluacion == null)
                throw new Exception("Evaluación no encontrada");
            if (!string.IsNullOrEmpty(dto.Titulo))
                evaluacion.Titulo = dto.Titulo;

            if (!string.IsNullOrEmpty(dto.Descripcion))
                evaluacion.Descripcion = dto.Descripcion;

            await _context.SaveChangesAsync();

            return evaluacion;
        }

        public async Task<Evaluacion> EliminarFisicoAsync(EvaluacionIdDto dto)
        {
            var evaluacion = await _context.Evaluaciones.FindAsync(dto.Id);
            if (evaluacion == null)
                throw new Exception("Evaluación no encontrada");

            _context.Evaluaciones.Remove(evaluacion);
            await _context.SaveChangesAsync();
            return evaluacion;
        }
    }
}
