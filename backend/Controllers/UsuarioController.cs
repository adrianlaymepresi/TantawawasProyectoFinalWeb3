using backend.Models.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/usuario")]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        [Authorize(Policy = "EsAdmin")]
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

        [Authorize(Policy = "EsAdmin")]
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

        [Authorize(Policy = "EsAdmin")]
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

        [Authorize(Policy = "EsAdmin")]
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

        [Authorize(Policy = "EsAdmin")]
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

        [Authorize(Policy = "EsAdmin")]
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

        [Authorize]
        [HttpPut("cambiarPasswordUsuario")]
        public async Task<IActionResult> CambiarPassword(UsuarioCambiarPasswordDto dto)
        {
            try
            {
                await _service.CambiarPasswordAsync(dto);
                return Ok(new { mensaje = "Contraseña cambiada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Policy = "EsAdmin")]
        [HttpPut("reestablecerPassword")]
        public async Task<IActionResult> ReestablecerPassword(UsuarioResetPasswordDto dto)
        {
            try
            {
                await _service.ReestablecerPasswordAsync(dto);
                return Ok(new { mensaje = "Contraseña restablecida correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Policy = "EsAdmin")]
        [HttpPut("activarUsuario")]
        public async Task<IActionResult> ActivarUsuario(UsuarioIdDto dto)
        {
            try
            {
                var usuario = await _service.ActivarAsync(dto);
                return Ok(new { mensaje = "Usuario activado correctamente", usuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize(Policy = "EsAdmin")]
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

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UsuarioLoginDto dto)
        {
            try
            {
                var token = await _service.LoginAsync(dto);
                return Ok(new { token, mensaje = "Login exitoso. Token establecido en cookie segura." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                _service.Logout();
                return Ok(new { mensaje = "Logout exitoso. Cookie eliminada." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
