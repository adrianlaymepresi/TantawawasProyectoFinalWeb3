using backend.Models.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly MaterialService _service;

        public MaterialController(MaterialService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var materiales = await _service.ObtenerTodosAsync();
                return Ok(materiales);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("curso/{cursoId}")]
        public async Task<IActionResult> ObtenerPorCurso(int cursoId)
        {
            try
            {
                var materiales = await _service.ObtenerPorCursoAsync(cursoId);
                return Ok(materiales);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var material = await _service.ObtenerPorIdAsync(new MaterialIdDto { Id = id });
                return Ok(material);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] MaterialCrearDto dto)
        {
            try
            {
                var material = await _service.CrearAsync(dto);
                return CreatedAtAction(nameof(ObtenerPorId), new { id = material.Id }, material);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] MaterialActualizarDto dto)
        {
            try
            {
                var material = await _service.ActualizarAsync(dto);
                return Ok(material);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var material = await _service.EliminarFisicoAsync(new MaterialIdDto { Id = id });
                return Ok(new { message = "Material eliminado exitosamente", material });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
