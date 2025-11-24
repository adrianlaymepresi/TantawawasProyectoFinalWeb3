using backend.Data;
using backend.Models;
using backend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class CursoService
    {
        private readonly AppDbContext _context;

        public CursoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Curso>> ObtenerTodosAsync()
        {
            return await _context.Cursos
                .Include(c => c.Docente)
                .ToListAsync();
        }

        public async Task<Curso> ObtenerPorIdAsync(CursoIdDto dto)
        {
            var curso = await _context.Cursos
                .Include(c => c.Docente)
                .FirstOrDefaultAsync(c => c.Id == dto.Id);

            if (curso == null)
                throw new Exception("Curso no encontrado");

            return curso;
        }

        public async Task<List<Curso>> BuscarPorNombreAsync(CursoBuscarPorNombreDto dto)
        {
            string nombre = dto.Nombre.Trim().ToLower();

            var cursos = await _context.Cursos
                .Where(c => c.Nombre.ToLower().Contains(nombre))
                .Include(c => c.Docente)
                .ToListAsync();

            if (!cursos.Any())
                throw new Exception("No se encontraron cursos con ese nombre");

            return cursos;
        }

        public async Task<Curso> CrearAsync(CursoCrearDto dto)
        {
            var docenteExiste = await _context.Usuarios.AnyAsync(u => u.Id == dto.DocenteId);
            if (!docenteExiste)
                throw new Exception("El docente asignado no existe");

            if (await _context.Cursos.AnyAsync(c => c.Nombre == dto.Nombre && c.DocenteId == dto.DocenteId))
                throw new Exception("Este docente ya tiene un curso con ese nombre");

            var curso = new Curso
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                DocenteId = dto.DocenteId
            };

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();
            return curso;
        }

        public async Task<Curso> ActualizarAsync(CursoActualizarDto dto)
        {
            var curso = await _context.Cursos.FindAsync(dto.Id);
            if (curso == null)
                throw new Exception("Curso no encontrado");

            var docenteExiste = await _context.Usuarios.AnyAsync(u => u.Id == dto.DocenteId);
            if (!docenteExiste)
                throw new Exception("El docente asignado no existe");

            if (await _context.Cursos.AnyAsync(c => c.Nombre == dto.Nombre && c.DocenteId == dto.DocenteId && c.Id != dto.Id))
                throw new Exception("Ya existe otro curso con ese nombre para ese docente");

            curso.Nombre = dto.Nombre;
            curso.Descripcion = dto.Descripcion;
            curso.DocenteId = dto.DocenteId;

            await _context.SaveChangesAsync();
            return curso;
        }

        public async Task<Curso> EliminarFisicoAsync(CursoIdDto dto)
        {
            var curso = await _context.Cursos.FindAsync(dto.Id);
            if (curso == null)
                throw new Exception("Curso no encontrado");

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();
            return curso;
        }
    }
}
