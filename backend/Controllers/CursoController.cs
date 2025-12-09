using backend.Models.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/curso")]
    [Authorize(Policy = "EsAdmin")]
    public class CursoController : ControllerBase
    {
        private readonly CursoService _service;

        public CursoController(CursoService service)
        {
            _service = service;
        }

        [HttpGet("obtenerCursos")]
        public async Task<IActionResult> ObtenerCursos()
        {
            try
            {
                var cursos = await _service.ObtenerTodosAsync();
                return Ok(cursos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("obtenerCursoPorId/{id}")]
        public async Task<IActionResult> ObtenerCursoPorId(int id)
        {
            try
            {
                var dto = new CursoIdDto { Id = id };
                var curso = await _service.ObtenerPorIdAsync(dto);
                return Ok(curso);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("buscarPorNombre/{nombre}")]
        public async Task<IActionResult> BuscarPorNombre(string nombre)
        {
            try
            {
                var cursos = await _service.BuscarPorNombreAsync(
                    new CursoBuscarPorNombreDto { Nombre = nombre }
                );
                return Ok(cursos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("crearCurso")]
        public async Task<IActionResult> CrearCurso(CursoCrearDto dto)
        {
            try
            {
                var curso = await _service.CrearAsync(dto);
                return Ok(new { mensaje = "Curso creado correctamente", curso });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("actualizarCurso")]
        public async Task<IActionResult> ActualizarCurso(CursoActualizarDto dto)
        {
            try
            {
                var curso = await _service.ActualizarAsync(dto);
                return Ok(new { mensaje = "Curso actualizado correctamente", curso });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("activarCurso")]
        public async Task<IActionResult> ActivarCurso(CursoIdDto dto)
        {
            try
            {
                var curso = await _service.ActivarAsync(dto);
                return Ok(new { mensaje = "Curso activado correctamente", curso });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("eliminarCursoLogico")]
        public async Task<IActionResult> EliminarCursoLogico(CursoIdDto dto)
        {
            try
            {
                var curso = await _service.EliminarLogicoAsync(dto);
                return Ok(new { mensaje = "Curso desactivado correctamente", curso });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
