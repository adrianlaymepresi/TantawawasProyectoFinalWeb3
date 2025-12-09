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

        public async Task<List<ResultadoEvaluacionObtenerDto>> ObtenerTodosAsync()
        {
            return await _context.ResultadosEvaluaciones
                .Include(r => r.Estudiante)
                .Include(r => r.Evaluacion)
                    .ThenInclude(e => e.Curso)
                .Select(r => new ResultadoEvaluacionObtenerDto
                {
                    Id = r.Id,
                    EstudianteId = r.EstudianteId,
                    EstudianteNombreCompleto = r.Estudiante != null ? r.Estudiante.Nombres + " " + r.Estudiante.Apellidos : "Estudiante no identificado",
                    EvaluacionId = r.EvaluacionId,
                    EvaluacionTitulo = r.Evaluacion != null ? r.Evaluacion.Titulo : "Evaluación no identificada",
                    CursoId = r.Evaluacion != null ? r.Evaluacion.CursoId : 0,
                    CursoNombre = r.Evaluacion != null && r.Evaluacion.Curso != null ? r.Evaluacion.Curso.Nombre : "Curso no identificado",
                    Nota = r.Nota
                })
                .ToListAsync();
        }

        public async Task<List<ResultadoEvaluacionSimpleDto>> ObtenerPorEvaluacionAsync(int evaluacionId)
        {
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

        public async Task<List<ResultadoPorEstudianteDto>> ObtenerPorEstudianteAsync(int estudianteId)
        {
            return await _context.ResultadosEvaluaciones
                .Include(r => r.Evaluacion)
                    .ThenInclude(e => e.Curso)
                .Where(r => r.EstudianteId == estudianteId)
                .OrderByDescending(r => r.Evaluacion.FechaCreacion)
                .Select(r => new ResultadoPorEstudianteDto
                {
                    Id = r.Id,
                    EvaluacionId = r.EvaluacionId,
                    EvaluacionTitulo = r.Evaluacion != null ? r.Evaluacion.Titulo : "Evaluación no identificada",
                    CursoId = r.Evaluacion != null ? r.Evaluacion.CursoId : 0,
                    CursoNombre = r.Evaluacion != null && r.Evaluacion.Curso != null ? r.Evaluacion.Curso.Nombre : "Curso no identificado",
                    FechaEvaluacion = r.Evaluacion != null ? r.Evaluacion.FechaCreacion : DateTime.MinValue,
                    Nota = r.Nota
                })
                .ToListAsync();
        }

        public async Task<ResultadoEvaluacionObtenerDto> ObtenerPorIdAsync(int id)
        {
            var resultado = await _context.ResultadosEvaluaciones
                .Include(r => r.Estudiante)
                .Include(r => r.Evaluacion)
                    .ThenInclude(e => e.Curso)
                .Where(r => r.Id == id)
                .Select(r => new ResultadoEvaluacionObtenerDto
                {
                    Id = r.Id,
                    EstudianteId = r.EstudianteId,
                    EstudianteNombreCompleto = r.Estudiante != null ? r.Estudiante.Nombres + " " + r.Estudiante.Apellidos : "Estudiante no identificado",
                    EvaluacionId = r.EvaluacionId,
                    EvaluacionTitulo = r.Evaluacion != null ? r.Evaluacion.Titulo : "Evaluación no identificada",
                    CursoId = r.Evaluacion != null ? r.Evaluacion.CursoId : 0,
                    CursoNombre = r.Evaluacion != null && r.Evaluacion.Curso != null ? r.Evaluacion.Curso.Nombre : "Curso no identificado",
                    Nota = r.Nota
                })
                .FirstOrDefaultAsync();

            if (resultado == null)
                throw new Exception("Resultado de evaluación no encontrado");

            return resultado;
        }

        public async Task<ResultadoEvaluacionObtenerDto> CrearAsync(ResultadoEvaluacionCrearDto dto)
        {
            var estudiante = await _context.Usuarios.FindAsync(dto.EstudianteId);
            if (estudiante == null)
                throw new Exception("El estudiante especificado no existe");

            var evaluacion = await _context.Evaluaciones
                .Include(e => e.Curso)
                .FirstOrDefaultAsync(e => e.Id == dto.EvaluacionId);
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

            return new ResultadoEvaluacionObtenerDto
            {
                Id = resultado.Id,
                EstudianteId = resultado.EstudianteId,
                EstudianteNombreCompleto = estudiante.Nombres + " " + estudiante.Apellidos,
                EvaluacionId = resultado.EvaluacionId,
                EvaluacionTitulo = evaluacion.Titulo,
                CursoId = evaluacion.CursoId,
                CursoNombre = evaluacion.Curso != null ? evaluacion.Curso.Nombre : "Curso no identificado",
                Nota = resultado.Nota
            };
        }

        public async Task<ResultadoEvaluacionObtenerDto> ActualizarAsync(ResultadoEvaluacionActualizarDto dto)
        {
            var resultado = await _context.ResultadosEvaluaciones
                .Include(r => r.Estudiante)
                .Include(r => r.Evaluacion)
                    .ThenInclude(e => e.Curso)
                .FirstOrDefaultAsync(r => r.Id == dto.Id);
            
            if (resultado == null)
                throw new Exception("Resultado de evaluación no encontrado");

            if (resultado.Estudiante == null)
                throw new Exception("El estudiante asignado a este resultado ya no existe");

            if (resultado.Evaluacion == null)
                throw new Exception("La evaluación asignada a este resultado ya no existe");

            resultado.Nota = dto.Nota;

            await _context.SaveChangesAsync();
            
            return new ResultadoEvaluacionObtenerDto
            {
                Id = resultado.Id,
                EstudianteId = resultado.EstudianteId,
                EstudianteNombreCompleto = resultado.Estudiante.Nombres + " " + resultado.Estudiante.Apellidos,
                EvaluacionId = resultado.EvaluacionId,
                EvaluacionTitulo = resultado.Evaluacion.Titulo,
                CursoId = resultado.Evaluacion.CursoId,
                CursoNombre = resultado.Evaluacion.Curso != null ? resultado.Evaluacion.Curso.Nombre : "Curso no identificado",
                Nota = resultado.Nota
            };
        }

        public async Task<ResultadoEvaluacionObtenerDto> EliminarFisicoAsync(int id)
        {
            var resultado = await _context.ResultadosEvaluaciones
                .Include(r => r.Estudiante)
                .Include(r => r.Evaluacion)
                    .ThenInclude(e => e.Curso)
                .FirstOrDefaultAsync(r => r.Id == id);
            
            if (resultado == null)
                throw new Exception("Resultado de evaluación no encontrado");

            var resultadoDto = new ResultadoEvaluacionObtenerDto
            {
                Id = resultado.Id,
                EstudianteId = resultado.EstudianteId,
                EstudianteNombreCompleto = resultado.Estudiante != null ? resultado.Estudiante.Nombres + " " + resultado.Estudiante.Apellidos : "Estudiante no identificado",
                EvaluacionId = resultado.EvaluacionId,
                EvaluacionTitulo = resultado.Evaluacion != null ? resultado.Evaluacion.Titulo : "Evaluación no identificada",
                CursoId = resultado.Evaluacion != null ? resultado.Evaluacion.CursoId : 0,
                CursoNombre = resultado.Evaluacion != null && resultado.Evaluacion.Curso != null ? resultado.Evaluacion.Curso.Nombre : "Curso no identificado",
                Nota = resultado.Nota
            };

            _context.ResultadosEvaluaciones.Remove(resultado);
            await _context.SaveChangesAsync();
            
            return resultadoDto;
        }

        public async Task<List<ResultadoPorCursoEstudianteDto>> ObtenerResultadosPorEstudianteYCursoAsync(int estudianteId, int cursoId)
        {
            return await _context.ResultadosEvaluaciones
                .Include(r => r.Evaluacion)
                .Where(r => r.EstudianteId == estudianteId && r.Evaluacion.CursoId == cursoId)
                .OrderByDescending(r => r.Evaluacion.FechaCreacion)
                .Select(r => new ResultadoPorCursoEstudianteDto
                {
                    Id = r.Id,
                    EvaluacionId = r.EvaluacionId,
                    EvaluacionTitulo = r.Evaluacion != null ? r.Evaluacion.Titulo : "Evaluación no identificada",
                    FechaEvaluacion = r.Evaluacion != null ? r.Evaluacion.FechaCreacion : DateTime.MinValue,
                    Nota = r.Nota
                })
                .ToListAsync();
        }
    }
}
