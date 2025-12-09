using backend.Models;
using backend.Models.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaterialController : ControllerBase
    {
        private readonly MaterialService _service;

        public MaterialController(MaterialService service)
        {
            _service = service;
        }

        [HttpGet("obtenerMateriales")]
        [Authorize(Policy = "EsAdmin")]
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
                var material = await _service.ObtenerPorIdAsync(id);
                return Ok(material);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("buscarMaterialPorTitulo/{cursoId}/{titulo}")]
        public async Task<IActionResult> BuscarPorTitulo(int cursoId, string titulo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(titulo))
                    return BadRequest(new { message = "El título de búsqueda no puede estar vacío" });

                var materiales = await _service.BuscarPorTituloAsync(cursoId, titulo);
                return Ok(materiales);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Administrador,Docente")]
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

        [Authorize(Policy = "EsDocente")]
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

        [Authorize(Policy = "EsDocente")]
        [HttpDelete("eliminarMaterial/{id}")]
        public async Task<IActionResult> EliminarMaterial(int id)
        {
            try
            {
                var material = await _service.EliminarFisicoAsync(id);
                return Ok(new { message = "Material eliminado exitosamente", material });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
