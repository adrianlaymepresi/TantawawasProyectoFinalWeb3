using backend.Data;
using backend.Models;
using backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class MaterialService
    {
        private readonly AppDbContext _context;

        public MaterialService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MaterialObtenerDto>> ObtenerTodosAsync()
        {
            return await _context.Materiales.Include(m => m.Curso)
                .Select(m => new MaterialObtenerDto
                {
                    Id = m.Id,
                    Titulo = m.Titulo,
                    ArchivoAdjunto = m.ArchivoAdjunto,
                    FechaCreacion = m.FechaCreacion,
                    CursoId = m.CursoId,
                    CursoNombre = m.Curso != null ? m.Curso.Nombre : "Curso no identificado"
                }).ToListAsync();
        }

        public async Task<List<MaterialSimpleDto>> ObtenerPorCursoAsync(int cursoId)
        {
            return await _context.Materiales
                .Where(m => m.CursoId == cursoId)
                .Select(m => new MaterialSimpleDto
                {
                    Id = m.Id,
                    Titulo = m.Titulo,
                    FechaCreacion = m.FechaCreacion
                })
                .ToListAsync();
        }

        public async Task<MaterialObtenerDto> ObtenerPorIdAsync(int id)
        {
            var material = await _context.Materiales
                .Include(m => m.Curso)
                .Where(m => m.Id == id)
                .Select(m => new MaterialObtenerDto
                {
                    Id = m.Id,
                    Titulo = m.Titulo,
                    ArchivoAdjunto = m.ArchivoAdjunto,
                    FechaCreacion = m.FechaCreacion,
                    CursoId = m.CursoId,
                    CursoNombre = m.Curso != null ? m.Curso.Nombre : "Curso no identificado"
                })
                .FirstOrDefaultAsync();

            if (material == null)
                throw new Exception("Material de curso no encontrado");

            return material;
        }

        public async Task<List<MaterialSimpleDto>> BuscarPorTituloAsync(int cursoId, string titulo)
        {
            var materiales = await _context.Materiales
                .Include(m => m.Curso)
                .Where(m => m.CursoId == cursoId && m.Titulo.ToLower().Contains(titulo.Trim().ToLower()))
                .OrderByDescending(m => m.FechaCreacion)
                .Select(m => new MaterialSimpleDto
                {
                    Id = m.Id,
                    Titulo = m.Titulo,
                    FechaCreacion = m.FechaCreacion
                })
                .ToListAsync();

            if (!materiales.Any())
                throw new Exception("No se encontraron materiales con ese título en este curso");

            return materiales;
        }

        public async Task<MaterialObtenerDto> CrearAsync(MaterialCrearDto dto)
        {
            var curso = await _context.Cursos.FindAsync(dto.CursoId);
            if (curso == null)
                throw new Exception("El curso especificado no existe");

            var material = new Material
            {
                Titulo = dto.Titulo,
                ArchivoAdjunto = dto.ArchivoAdjunto,
                FechaCreacion = dto.FechaCreacion,
                CursoId = dto.CursoId
            };

            _context.Materiales.Add(material);
            await _context.SaveChangesAsync();

            return new MaterialObtenerDto
            {
                Id = material.Id,
                Titulo = material.Titulo,
                ArchivoAdjunto = material.ArchivoAdjunto,
                FechaCreacion = material.FechaCreacion,
                CursoId = material.CursoId,
                CursoNombre = curso.Nombre
            };
        }

        public async Task<MaterialObtenerDto> ActualizarAsync(MaterialActualizarDto dto)
        {
            var material = await _context.Materiales
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.Id == dto.Id);
            
            if (material == null)
                throw new Exception("Material de curso no encontrado");

            if (material.Curso == null)
                throw new Exception("El curso asignado al material ya no existe");

            if (!string.IsNullOrEmpty(dto.Titulo))
                material.Titulo = dto.Titulo;

            if (dto.ArchivoAdjunto != null)
                material.ArchivoAdjunto = dto.ArchivoAdjunto;

            await _context.SaveChangesAsync();
            
            return new MaterialObtenerDto
            {
                Id = material.Id,
                Titulo = material.Titulo,
                ArchivoAdjunto = material.ArchivoAdjunto,
                FechaCreacion = material.FechaCreacion,
                CursoId = material.CursoId,
                CursoNombre = material.Curso.Nombre
            };
        }

        public async Task<MaterialObtenerDto> EliminarFisicoAsync(int id)
        {
            var material = await _context.Materiales
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (material == null)
                throw new Exception("Material de curso no encontrado");

            var materialDto = new MaterialObtenerDto
            {
                Id = material.Id,
                Titulo = material.Titulo,
                ArchivoAdjunto = material.ArchivoAdjunto,
                FechaCreacion = material.FechaCreacion,
                CursoId = material.CursoId,
                CursoNombre = material.Curso != null ? material.Curso.Nombre : "Curso no identificado"
            };

            _context.Materiales.Remove(material);
            await _context.SaveChangesAsync();
            
            return materialDto;
        }
    }
}
