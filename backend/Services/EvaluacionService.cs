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

        public async Task<List<EvaluacionObtenerDto>> ObtenerTodosAsync()
        {
            return await _context.Evaluaciones
                .Include(e => e.Curso)
                .Include(e => e.Resultados)
                .OrderByDescending(e => e.FechaCreacion)
                .Select(e => new EvaluacionObtenerDto
                {
                    Id = e.Id,
                    Titulo = e.Titulo,
                    Descripcion = e.Descripcion,
                    FechaCreacion = e.FechaCreacion,
                    CursoId = e.CursoId,
                    CursoNombre = e.Curso != null ? e.Curso.Nombre : "Curso no identificado",
                    CantidadResultados = e.Resultados.Count,
                    PromedioNotas = e.Resultados.Any() ? e.Resultados.Average(r => r.Nota) : 0
                })
                .ToListAsync();
        }

        public async Task<EvaluacionObtenerDto> ObtenerPorIdAsync(int id)
        {
            var evaluacion = await _context.Evaluaciones
                .Include(e => e.Curso)
                .Include(e => e.Resultados)
                .Where(e => e.Id == id)
                .Select(e => new EvaluacionObtenerDto
                {
                    Id = e.Id,
                    Titulo = e.Titulo,
                    Descripcion = e.Descripcion,
                    FechaCreacion = e.FechaCreacion,
                    CursoId = e.CursoId,
                    CursoNombre = e.Curso != null ? e.Curso.Nombre : "Curso no identificado",
                    CantidadResultados = e.Resultados.Count,
                    PromedioNotas = e.Resultados.Any() ? e.Resultados.Average(r => r.Nota) : 0
                })
                .FirstOrDefaultAsync();

            if (evaluacion == null)
                throw new Exception("Evaluación no encontrada");

            return evaluacion;
        }

        public async Task<List<EvaluacionSimpleDto>> ObtenerPorCursoAsync(int cursoId)
        {
            return await _context.Evaluaciones
                .Include(e => e.Resultados)
                .Where(e => e.CursoId == cursoId)
                .OrderByDescending(e => e.FechaCreacion)
                .Select(e => new EvaluacionSimpleDto
                {
                    Id = e.Id,
                    Titulo = e.Titulo,
                    Descripcion = e.Descripcion,
                    FechaCreacion = e.FechaCreacion,
                    CantidadResultados = e.Resultados.Count,
                    PromedioNotas = e.Resultados.Any() ? e.Resultados.Average(r => r.Nota) : 0
                })
                .ToListAsync();
        }

        public async Task<List<EvaluacionSimpleDto>> BuscarPorTituloAsync(int cursoId, string titulo)
        {
            var evaluaciones = await _context.Evaluaciones
                .Include(e => e.Resultados)
                .Where(e => e.CursoId == cursoId && e.Titulo.ToLower().Contains(titulo.Trim().ToLower()))
                .OrderByDescending(e => e.FechaCreacion)
                .Select(e => new EvaluacionSimpleDto
                {
                    Id = e.Id,
                    Titulo = e.Titulo,
                    Descripcion = e.Descripcion,
                    FechaCreacion = e.FechaCreacion,
                    CantidadResultados = e.Resultados.Count,
                    PromedioNotas = e.Resultados.Any() ? e.Resultados.Average(r => r.Nota) : 0
                })
                .ToListAsync();

            if (!evaluaciones.Any())
                throw new Exception("No se encontraron evaluaciones con ese título en este curso");

            return evaluaciones;
        }

        public async Task<List<ResultadoEvaluacionSimpleDto>> ObtenerResultadosPorEvaluacionAsync(int evaluacionId)
        {
            var evaluacionExiste = await _context.Evaluaciones.AnyAsync(e => e.Id == evaluacionId);
            if (!evaluacionExiste)
                throw new Exception("La evaluación especificada no existe");

            return await _context.ResultadosEvaluaciones
                .Include(r => r.Estudiante)
                .Where(r => r.EvaluacionId == evaluacionId)
                .OrderByDescending(r => r.Nota)
                .Select(r => new ResultadoEvaluacionSimpleDto
                {
                    Id = r.Id,
                    EstudianteId = r.EstudianteId,
                    EstudianteNombreCompleto = r.Estudiante != null ? r.Estudiante.Nombres + " " + r.Estudiante.Apellidos : "Estudiante no identificado",
                    Nota = r.Nota
                })
                .ToListAsync();
        }

        public async Task<EvaluacionObtenerDto> CrearAsync(EvaluacionCrearDto dto)
        {
            var curso = await _context.Cursos.FindAsync(dto.CursoId);
            if (curso == null)
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

            return new EvaluacionObtenerDto
            {
                Id = evaluacion.Id,
                Titulo = evaluacion.Titulo,
                Descripcion = evaluacion.Descripcion,
                FechaCreacion = evaluacion.FechaCreacion,
                CursoId = evaluacion.CursoId,
                CursoNombre = curso.Nombre,
                CantidadResultados = 0,
                PromedioNotas = 0
            };
        }

        public async Task<EvaluacionObtenerDto> ActualizarAsync(EvaluacionActualizarDto dto)
        {
            var evaluacion = await _context.Evaluaciones
                .Include(e => e.Curso)
                .Include(e => e.Resultados)
                .FirstOrDefaultAsync(e => e.Id == dto.Id);
            
            if (evaluacion == null)
                throw new Exception("Evaluación no encontrada");

            if (evaluacion.Curso == null)
                throw new Exception("El curso asignado a la evaluación ya no existe");

            if (!string.IsNullOrEmpty(dto.Titulo))
                evaluacion.Titulo = dto.Titulo;

            if (!string.IsNullOrEmpty(dto.Descripcion))
                evaluacion.Descripcion = dto.Descripcion;

            await _context.SaveChangesAsync();

            return new EvaluacionObtenerDto
            {
                Id = evaluacion.Id,
                Titulo = evaluacion.Titulo,
                Descripcion = evaluacion.Descripcion,
                FechaCreacion = evaluacion.FechaCreacion,
                CursoId = evaluacion.CursoId,
                CursoNombre = evaluacion.Curso.Nombre,
                CantidadResultados = evaluacion.Resultados.Count,
                PromedioNotas = evaluacion.Resultados.Any() ? evaluacion.Resultados.Average(r => r.Nota) : 0
            };
        }

        public async Task<EvaluacionObtenerDto> EliminarFisicoAsync(int id)
        {
            var evaluacion = await _context.Evaluaciones
                .Include(e => e.Curso)
                .Include(e => e.Resultados)
                .FirstOrDefaultAsync(e => e.Id == id);
            
            if (evaluacion == null)
                throw new Exception("Evaluación no encontrada");

            var evaluacionDto = new EvaluacionObtenerDto
            {
                Id = evaluacion.Id,
                Titulo = evaluacion.Titulo,
                Descripcion = evaluacion.Descripcion,
                FechaCreacion = evaluacion.FechaCreacion,
                CursoId = evaluacion.CursoId,
                CursoNombre = evaluacion.Curso != null ? evaluacion.Curso.Nombre : "Curso no identificado",
                CantidadResultados = evaluacion.Resultados.Count,
                PromedioNotas = evaluacion.Resultados.Any() ? evaluacion.Resultados.Average(r => r.Nota) : 0
            };

            _context.Evaluaciones.Remove(evaluacion);
            await _context.SaveChangesAsync();
            
            return evaluacionDto;
        }
    }
}
