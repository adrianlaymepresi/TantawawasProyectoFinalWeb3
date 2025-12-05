using backend.Data;
using backend.Models;
using backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<MaterialObtenerDto> ObtenerPorIdAsync(MaterialIdDto dto)
        {
            var material = await _context.Materiales
                .Include(m => m.Curso)
                .Where(m => m.Id == dto.Id)
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

        public async Task<List<Material>> BuscarPorTituloAsync(string titulo)
        {
            var materiales = await _context.Materiales
                .Where(m => m.Titulo.ToLower().Contains(titulo.Trim().ToLower()))
                .Include(m => m.Curso)
                .OrderByDescending(m => m.FechaCreacion)
                .ToListAsync();

            if (!materiales.Any())
                throw new Exception("No se encontraron materiales con ese título");

            return materiales;
        }

        public async Task<Material> CrearAsync(MaterialCrearDto dto)
        {
            var cursoExiste = await _context.Cursos.AnyAsync(c => c.Id == dto.CursoId);
            if (!cursoExiste)
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

            return material;
        }

        public async Task<Material> ActualizarAsync(MaterialActualizarDto dto)
        {
            var material = await _context.Materiales.FindAsync(dto.Id);
            if (material == null)
                throw new Exception("Material de curso no encontrado");

            var cursoExiste = await _context.Cursos.AnyAsync(c => c.Id == material.CursoId);
            if (!cursoExiste)
                throw new Exception("El curso asignado al material ya no existe");

            if (!string.IsNullOrEmpty(dto.Titulo))
                material.Titulo = dto.Titulo;

            if (dto.ArchivoAdjunto != null)
                material.ArchivoAdjunto = dto.ArchivoAdjunto;

            await _context.SaveChangesAsync();
            return material;
        }

        public async Task<Material> EliminarFisicoAsync(MaterialIdDto dto)
        {
            var material = await _context.Materiales.FindAsync(dto.Id);
            if (material == null)
                throw new Exception("Material de curso no encontrado");

            _context.Materiales.Remove(material);
            await _context.SaveChangesAsync();
            return material;
        }
    }
}
