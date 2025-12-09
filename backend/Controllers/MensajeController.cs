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
    public class MensajeController : ControllerBase
    {
        private readonly MensajeService _service;

        public MensajeController(MensajeService service)
        {
            _service = service;
        }

        [Authorize(Policy = "EsAdmin")]
        [HttpGet("obtenerMensajes")]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var mensajes = await _service.ObtenerTodosAsync();
                return Ok(mensajes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("obtenerMensajesPorCurso/{cursoId}")]
        public async Task<IActionResult> ObtenerPorCurso(int cursoId)
        {
            try
            {
                var mensajes = await _service.ObtenerPorCursoAsync(cursoId);
                return Ok(mensajes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Policy = "EsAdmin")]
        [HttpGet("obtenerMensajesPorUsuario/{usuarioId}")]
        public async Task<IActionResult> ObtenerPorUsuario(int usuarioId)
        {
            try
            {
                var mensajes = await _service.ObtenerPorUsuarioAsync(usuarioId);
                return Ok(mensajes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("obtenerMensajePorId/{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var mensaje = await _service.ObtenerPorIdAsync(id);
                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Policy = "EsDocente")]
        [HttpPost("crearMensaje")]
        public async Task<IActionResult> CrearMensaje(MensajeCrearDto dto)
        {
            try
            {
                var mensaje = await _service.CrearAsync(dto);
                return Ok(new { message = "Mensaje creado", mensaje });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Policy = "EsDocente")]
        [HttpPut("actualizarMensaje")]
        public async Task<IActionResult> ActualizarMensaje(MensajeActualizarDto dto)
        {
            try
            {
                var mensaje = await _service.ActualizarAsync(dto);
                return Ok(new { message = "Mensaje actualizado", mensaje });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Policy = "EsDocente")]
        [HttpDelete("eliminarMensaje/{id}")]
        public async Task<IActionResult> EliminarMensaje(int id)
        {
            try
            {
                var mensaje = await _service.EliminarFisicoAsync(id);
                return Ok(new { message = "Mensaje eliminado exitosamente", mensaje });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
