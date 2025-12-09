using backend.Models.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ResultadoEvaluacionController : ControllerBase
    {
        private readonly ResultadoEvaluacionService _service;

        public ResultadoEvaluacionController(ResultadoEvaluacionService service)
        {
            _service = service;
        }

        [Authorize(Policy = "EsAdmin")]
        [HttpGet("obtenerResultados")]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var resultados = await _service.ObtenerTodosAsync();
                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Policy = "EsDocente")]
        [HttpGet("obtenerResultadosPorEvaluacion/{evaluacionId}")]
        public async Task<IActionResult> ObtenerPorEvaluacion(int evaluacionId)
        {
            try
            {
                var resultados = await _service.ObtenerPorEvaluacionAsync(evaluacionId);
                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("obtenerResultadosPorEstudiante/{estudianteId}")]
        public async Task<IActionResult> ObtenerPorEstudiante(int estudianteId)
        {
            try
            {
                var resultados = await _service.ObtenerPorEstudianteAsync(estudianteId);
                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("obtenerResultadosEstudiantePorCurso/{estudianteId}/{cursoId}")]
        public async Task<IActionResult> ObtenerPorEstudianteYCurso(int estudianteId, int cursoId)
        {
            try
            {
                var resultados = await _service.ObtenerResultadosPorEstudianteYCursoAsync(estudianteId, cursoId);
                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("obtenerResultadoPorId/{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var resultado = await _service.ObtenerPorIdAsync(id);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Administrador,Docente")]
        [HttpPost("crearResultado")]
        public async Task<IActionResult> CrearResultado(ResultadoEvaluacionCrearDto dto)
        {
            try
            {
                var resultado = await _service.CrearAsync(dto);
                return Ok(new { message = "Resultado de evaluación creado", resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Policy = "EsDocente")]
        [HttpPut("actualizarResultado")]
        public async Task<IActionResult> ActualizarResultado(ResultadoEvaluacionActualizarDto dto)
        {
            try
            {
                var resultado = await _service.ActualizarAsync(dto);
                return Ok(new { message = "Resultado de evaluación actualizado", resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Policy = "EsDocente")]
        [HttpDelete("eliminarResultado/{id}")]
        public async Task<IActionResult> EliminarResultado(int id)
        {
            try
            {
                var resultado = await _service.EliminarFisicoAsync(id);
                return Ok(new { message = "Resultado de evaluación eliminado exitosamente", resultado });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
