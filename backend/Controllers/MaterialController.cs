using backend.Models;
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

        [HttpGet("obtenerMateriales")]
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

        [HttpGet("obtenerMaterialesPorCurso/{cursoId}")]
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

        [HttpGet("obtenerMaterialPorId/{id}")]
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

        [HttpPost("crearMaterial")]
        public async Task<IActionResult> CrearMaterial(MaterialCrearDto dto)
        {
            try
            {
                var material = await _service.CrearAsync(dto);
                return Ok(new { message = "Material de curso creado", material });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("actualizarMaterial")]
        public async Task<IActionResult> ActualizarMaterial(MaterialActualizarDto dto)
        {
            try
            {
                var material = await _service.ActualizarAsync(dto);
                return Ok(new { message = "Material de curso actualizado", material });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("eliminarMaterial")]
        public async Task<IActionResult> Eliminar(MaterialIdDto dto)
        {
            try
            {
                var material = await _service.EliminarFisicoAsync(dto);
                return Ok(new { message = "Material eliminado exitosamente", material });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
