using backend.Models.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/inscripcion")]
    [Authorize(Policy = "EsAdmin")]
    public class InscripcionController : ControllerBase
    {
        private readonly InscripcionService _service;

        public InscripcionController(InscripcionService service)
        {
            _service = service;
        }

        [HttpGet("obtenerInscripciones")]
        public async Task<IActionResult> ObtenerInscripciones()
        {
            try
            {
                var inscripciones = await _service.ObtenerTodasAsync();
                return Ok(inscripciones);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("obtenerInscripcionPorId/{id}")]
        public async Task<IActionResult> ObtenerInscripcionPorId(int id)
        {
            try
            {
                var dto = new InscripcionIdDto { Id = id };
                var inscripcion = await _service.ObtenerPorIdAsync(dto);
                return Ok(inscripcion);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("buscarCursosPorEstudiante/{estudianteId}")]
        public async Task<IActionResult> BuscarCursosPorEstudiante(int estudianteId)
        {
            try
            {
                var inscripciones = await _service.BuscarPorEstudianteAsync(
                    new InscripcionBuscarPorEstudianteDto { EstudianteId = estudianteId }
                );
                return Ok(inscripciones);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("buscarEstudiantesPorCurso/{cursoId}")]
        public async Task<IActionResult> BuscarEstudiantesPorCurso(int cursoId)
        {
            try
            {
                var inscripciones = await _service.BuscarPorCursoAsync(
                    new InscripcionBuscarPorCursoDto { CursoId = cursoId }
                );
                return Ok(inscripciones);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("crearInscripcion")]
        public async Task<IActionResult> CrearInscripcion(InscripcionCrearDto dto)
        {
            try
            {
                var inscripcion = await _service.CrearAsync(dto);
                return Ok(new { mensaje = "Inscripción creada correctamente", inscripcion });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("actualizarInscripcion")]
        public async Task<IActionResult> ActualizarInscripcion(InscripcionActualizarDto dto)
        {
            try
            {
                var inscripcion = await _service.ActualizarAsync(dto);
                return Ok(new { mensaje = "Inscripción actualizada correctamente", inscripcion });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("eliminarInscripcion")]
        public async Task<IActionResult> EliminarInscripcion(InscripcionIdDto dto)
        {
            try
            {
                var inscripcion = await _service.EliminarFisicoAsync(dto);
                return Ok(new { mensaje = "Inscripción eliminada correctamente", inscripcion });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
