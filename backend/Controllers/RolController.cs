using backend.Models.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "EsAdmin")]
    public class RolController : ControllerBase
    {
        private readonly RolService _service;

        public RolController(RolService service)
        {
            _service = service;
        }

        [HttpGet("obtenerRoles")]
        public async Task<IActionResult> Obtener()
        {
            try
            {
                var roles = await _service.ObtenerTodosAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("obtenerRolPorId/{id}")]
        public async Task<IActionResult> ObtenerPorIdGet(int id)
        {
            try
            {
                var dto = new RolIdDto { Id = id };
                var rol = await _service.ObtenerPorIdAsync(dto);
                return Ok(rol);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("crearRol")]
        public async Task<IActionResult> Crear(RolCrearDto dto)
        {
            try
            {
                var rol = await _service.CrearAsync(dto);
                return Ok(new { mensaje = "Rol creado correctamente", rol });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("actualizarRol")]
        public async Task<IActionResult> Actualizar(RolActualizarDto dto)
        {
            try
            {
                var rol = await _service.ActualizarAsync(dto);
                return Ok(new { mensaje = "Rol actualizado correctamente", rol });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("eliminarRol")]
        public async Task<IActionResult> Eliminar(RolIdDto dto)
        {
            try
            {
                var rol = await _service.EliminarFisicoAsync(dto);
                return Ok(new { mensaje = "Rol eliminado correctamente", rol });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
