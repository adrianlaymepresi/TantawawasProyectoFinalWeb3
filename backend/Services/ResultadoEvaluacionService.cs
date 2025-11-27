using backend.Data;
using backend.Models;
using backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class ResultadoEvaluacionService
    {
        private readonly AppDbContext _context;

        public ResultadoEvaluacionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ResultadoEvaluacion>> ObtenerTodosAsync()
        {
            return await _context.ResultadosEvaluaciones
                .Include(r => r.Estudiante)
                .Include(r => r.Evaluacion)
                .ToListAsync();
        }

        public async Task<List<ResultadoEvaluacion>> ObtenerPorEvaluacionAsync(int evaluacionId)
        {
            return await _context.ResultadosEvaluaciones
                .Where(r => r.EvaluacionId == evaluacionId)
                .Include(r => r.Estudiante)
                .Include(r => r.Evaluacion)
                .OrderByDescending(r => r.Nota)
                .ToListAsync();
        }

        public async Task<List<ResultadoEvaluacion>> ObtenerPorEstudianteAsync(int estudianteId)
        {
            return await _context.ResultadosEvaluaciones
                .Where(r => r.EstudianteId == estudianteId)
                .Include(r => r.Estudiante)
                .Include(r => r.Evaluacion)
                .OrderByDescending(r => r.Evaluacion.FechaCreacion)
                .ToListAsync();
        }

        public async Task<ResultadoEvaluacion> ObtenerPorIdAsync(ResultadoEvaluacionIdDto dto)
        {
            var resultado = await _context.ResultadosEvaluaciones
                .Include(r => r.Estudiante)
                .Include(r => r.Evaluacion)
                    .ThenInclude(e => e.Curso)
                .FirstOrDefaultAsync(r => r.Id == dto.Id);

            if (resultado == null)
                throw new Exception("Resultado de evaluación no encontrado");

            return resultado;
        }

        public async Task<ResultadoEvaluacion> CrearAsync(ResultadoEvaluacionCrearDto dto)
        {
            var estudianteExiste = await _context.Usuarios.AnyAsync(u => u.Id == dto.EstudianteId);
            if (!estudianteExiste)
                throw new Exception("El estudiante especificado no existe");

            var evaluacion = await _context.Evaluaciones.FindAsync(dto.EvaluacionId);
            if (evaluacion == null)
                throw new Exception("La evaluación especificada no existe");

            var inscripcionExistente = await _context.Inscripciones
                .AnyAsync(i => i.EstudianteId == dto.EstudianteId && i.CursoId == evaluacion.CursoId);
            if (!inscripcionExistente)
                throw new Exception("El estudiante especificado no está inscrito en este curso");

            var resultadoExistente = await _context.ResultadosEvaluaciones
                .AnyAsync(r => r.EstudianteId == dto.EstudianteId && r.EvaluacionId == dto.EvaluacionId);
            if (resultadoExistente)
                throw new Exception("Ya existe un resultado para este estudiante en esta evaluación");

            var resultado = new ResultadoEvaluacion
            {
                EstudianteId = dto.EstudianteId,
                EvaluacionId = dto.EvaluacionId,
                Nota = dto.Nota
            };

            _context.ResultadosEvaluaciones.Add(resultado);
            await _context.SaveChangesAsync();

            return resultado;
        }

        public async Task<ResultadoEvaluacion> ActualizarAsync(ResultadoEvaluacionActualizarDto dto)
        {
            var resultado = await _context.ResultadosEvaluaciones.FindAsync(dto.Id);
            if (resultado == null)
                throw new Exception("Resultado de evaluación no encontrado");

            var estudianteExiste = await _context.Usuarios.AnyAsync(u => u.Id == resultado.EstudianteId);
            if (!estudianteExiste)
                throw new Exception("El estudiante asignado a este resultado ya no existe");

            var evaluacionExiste = await _context.Evaluaciones.AnyAsync(e => e.Id == resultado.EvaluacionId);
            if (!evaluacionExiste)
                throw new Exception("La evaluación asignada a este resultado ya no existe");

            resultado.Nota = dto.Nota;

            await _context.SaveChangesAsync();
            return resultado;
        }

        public async Task<ResultadoEvaluacion> EliminarFisicoAsync(ResultadoEvaluacionIdDto dto)
        {
            var resultado = await _context.ResultadosEvaluaciones.FindAsync(dto.Id);
            if (resultado == null)
                throw new Exception("Resultado de evaluación no encontrado");

            _context.ResultadosEvaluaciones.Remove(resultado);
            await _context.SaveChangesAsync();
            return resultado;
        }
    }
}
