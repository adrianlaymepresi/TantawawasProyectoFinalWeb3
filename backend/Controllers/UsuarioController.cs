using backend.Models.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        [HttpGet("obtenerUsuarios")]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            try
            {
                var usuarios = await _service.ObtenerTodosAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("obtenerUsuarioPorId/{id}")]
        public async Task<IActionResult> ObtenerUsuarioPorId(int id)
        {
            try
            {
                var dto = new UsuarioIdDto { Id = id };
                var usuario = await _service.ObtenerPorIdAsync(dto);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("buscarPorCI/{ci}")]
        public async Task<IActionResult> BuscarPorCI(string ci)
        {
            try
            {
                var usuarios = await _service.BuscarPorCarnetIdentidadAsync(
                    new UsuarioBuscarPorCI { CarnetIdentidad = ci }
                );
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("buscarPorNombreCompleto/{nombre}")]
        public async Task<IActionResult> BuscarPorNombreCompleto(string nombre)
        {
            try
            {
                var usuarios = await _service.BuscarPorNombreCompletoAsync(
                    new UsuarioBuscarPorNombreCompleto { NombreCompleto = nombre }
                );
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("crearUsuario")]
        public async Task<IActionResult> CrearUsuario(UsuarioCrearDto dto)
        {
            try
            {
                var usuario = await _service.CrearAsync(dto);
                return Ok(new { mensaje = "Usuario creado correctamente", usuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("actualizarUsuario")]
        public async Task<IActionResult> ActualizarUsuario(UsuarioActualizarDto dto)
        {
            try
            {
                var usuario = await _service.ActualizarAsync(dto);
                return Ok(new { mensaje = "Usuario actualizado correctamente", usuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("actualizarPassword")]
        public async Task<IActionResult> ActualizarPassword(UsuarioActualizarPasswordDto dto)
        {
            try
            {
                var usuario = await _service.ActualizarPasswordAsync(dto);
                return Ok(new { mensaje = "Password actualizado correctamente", usuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("eliminarUsuarioLogico")]
        public async Task<IActionResult> EliminarUsuarioLogico(UsuarioIdDto dto)
        {
            try
            {
                var usuario = await _service.EliminarLogicoAsync(dto);
                return Ok(new { mensaje = "Usuario desactivado", usuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("eliminarUsuarioFisico")]
        public async Task<IActionResult> EliminarUsuarioFisico(UsuarioIdDto dto)
        {
            try
            {
                var usuario = await _service.EliminarFisicoAsync(dto);
                return Ok(new { mensaje = "Usuario eliminado permanentemente", usuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
