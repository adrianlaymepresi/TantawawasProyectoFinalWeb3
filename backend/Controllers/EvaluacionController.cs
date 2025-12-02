using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models.DTOs;
using backend.Services;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EvaluacionController : ControllerBase
    {
        private readonly EvaluacionService _service;

        public EvaluacionController(EvaluacionService service)
        {
            _service = service;
        }

        [HttpGet("obtenerEvaluaciones")]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var evaluaciones = await _service.ObtenerTodosAsync();
                return Ok(evaluaciones);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("obtenerEvaluacionesPorCurso/{cursoId}")]
        public async Task<IActionResult> ObtenerPorCurso(int cursoId)
        {
            try
            {
                var evaluaciones = await _service.ObtenerPorCursoAsync(cursoId);
                return Ok(evaluaciones);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("obtenerEvaluacionPorId/{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var evaluacion = await _service.ObtenerPorIdAsync(new EvaluacionIdDto { Id = id });
                return Ok(evaluacion);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("buscarEvaluacionPorTitulo/{titulo}")]
        public async Task<IActionResult> BuscarPorTitulo(string titulo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(titulo))
                    return BadRequest(new { message = "El título de búsqueda no puede estar vacío" });

                var evaluaciones = await _service.BuscarPorTituloAsync(titulo);
                return Ok(evaluaciones);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("crearEvaluacion")]
        public async Task<IActionResult> CrearEvaluacion(EvaluacionCrearDto dto)
        {
            try
            {
                var evaluacion = await _service.CrearAsync(dto);
                return Ok(new { message = "Evaluación creada", evaluacion });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("actualizarEvaluacion")]
        public async Task<IActionResult> ActualizarEvaluacion(EvaluacionActualizarDto dto)
        {
            try
            {
                var evaluacion = await _service.ActualizarAsync(dto);
                return Ok(new { message = "Evaluación actualizada", evaluacion });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("eliminarEvaluacion")]
        public async Task<IActionResult> EliminarEvaluacion(EvaluacionIdDto dto)
        {
            try
            {
                var evaluacion = await _service.EliminarFisicoAsync(dto);
                return Ok(new { message = "Evaluación eliminada exitosamente", evaluacion });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
