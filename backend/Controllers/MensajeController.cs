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
                var mensaje = await _service.ObtenerPorIdAsync(new MensajeIdDto { Id = id });
                return Ok(mensaje);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

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

        [HttpDelete("eliminarMensaje")]
        public async Task<IActionResult> EliminarMensaje(MensajeIdDto dto)
        {
            try
            {
                var mensaje = await _service.EliminarFisicoAsync(dto);
                return Ok(new { message = "Mensaje eliminado exitosamente", mensaje });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
