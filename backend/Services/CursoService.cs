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

        public async Task<List<CursoDocenteObtenerDto>> ObtenerTodosAsync()
        {
            var cursos = await _context.Cursos
                .Include(c => c.Docente)
                .Select(c => new CursoDocenteObtenerDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    DocenteId = c.DocenteId,
                    Nombres = c.Docente != null ? c.Docente.Nombres : "Desconocido",
                    Apellidos = c.Docente != null ? c.Docente.Apellidos : "Desconocido",
                    CarnetIdentidad = c.Docente != null ? c.Docente.CarnetIdentidad : 0,
                    EsActivo = c.EsActivo
                })
                .ToListAsync();

            return cursos;
        }

        public async Task<CursoDocenteObtenerDto> ObtenerPorIdAsync(CursoIdDto dto)
        {
            var curso = await _context.Cursos
                .Include(c => c.Docente)
                .Where(c => c.Id == dto.Id)
                .Select(c => new CursoDocenteObtenerDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    DocenteId = c.DocenteId,
                    Nombres = c.Docente != null ? c.Docente.Nombres : "Desconocido",
                    Apellidos = c.Docente != null ? c.Docente.Apellidos : "Desconocido",
                    CarnetIdentidad = c.Docente != null ? c.Docente.CarnetIdentidad : 0,
                    EsActivo = c.EsActivo
                })
                .FirstOrDefaultAsync();

            if (curso == null)
                throw new Exception("Curso no encontrado");

            return curso;
        }

        public async Task<List<CursoDocenteObtenerDto>> BuscarPorNombreAsync(CursoBuscarPorNombreDto dto)
        {
            string nombre = dto.Nombre.Trim().ToLower();

            var cursos = await _context.Cursos
                .Include(c => c.Docente)
                .Where(c => c.Nombre.ToLower().Contains(nombre))
                .Select(c => new CursoDocenteObtenerDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    DocenteId = c.DocenteId,
                    Nombres = c.Docente != null ? c.Docente.Nombres : "Desconocido",
                    Apellidos = c.Docente != null ? c.Docente.Apellidos : "Desconocido",
                    CarnetIdentidad = c.Docente != null ? c.Docente.CarnetIdentidad : 0,
                    EsActivo = c.EsActivo
                })
                .ToListAsync();

            if (!cursos.Any())
                throw new Exception("No se encontraron cursos con ese nombre");

            return cursos;
        }

        public async Task<CursoDocenteObtenerDto> CrearAsync(CursoCrearDto dto)
        {
            var docente = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == dto.DocenteId);

            if (docente == null)
                throw new Exception("El docente asignado no existe");

            if (docente.Rol == null || docente.Rol.NombreRol != "Docente")
                throw new Exception("El usuario asignado no tiene el rol de Docente");

            bool cursoDuplicado = await _context.Cursos
                .AnyAsync(c => c.Nombre == dto.Nombre && c.DocenteId == dto.DocenteId);

            if (cursoDuplicado)
                throw new Exception("Este docente ya tiene un curso con ese nombre");

            var curso = new Curso
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                DocenteId = dto.DocenteId
            };

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            return new CursoDocenteObtenerDto
            {
                Id = curso.Id,
                Nombre = curso.Nombre,
                Descripcion = curso.Descripcion,
                DocenteId = docente.Id,
                Nombres = docente.Nombres,
                Apellidos = docente.Apellidos,
                CarnetIdentidad = docente.CarnetIdentidad,
                EsActivo = curso.EsActivo
            };
        }

        public async Task<CursoDocenteObtenerDto> ActualizarAsync(CursoActualizarDto dto)
        {
            var curso = await _context.Cursos.FindAsync(dto.Id);
            if (curso == null)
                throw new Exception("Curso no encontrado");

            var docente = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == dto.DocenteId);

            if (docente == null)
                throw new Exception("El docente asignado no existe");

            if (docente.Rol == null || docente.Rol.NombreRol != "Docente")
                throw new Exception("El usuario asignado no tiene el rol de Docente");

            bool duplicado = await _context.Cursos
                .AnyAsync(c => c.Nombre == dto.Nombre &&
                               c.DocenteId == dto.DocenteId &&
                               c.Id != dto.Id);

            if (duplicado)
                throw new Exception("Ya existe otro curso con ese nombre para ese docente");

            curso.Nombre = dto.Nombre;
            curso.Descripcion = dto.Descripcion;
            curso.DocenteId = dto.DocenteId;

            await _context.SaveChangesAsync();

            return new CursoDocenteObtenerDto
            {
                Id = curso.Id,
                Nombre = curso.Nombre,
                Descripcion = curso.Descripcion,
                DocenteId = docente.Id,
                Nombres = docente.Nombres,
                Apellidos = docente.Apellidos,
                CarnetIdentidad = docente.CarnetIdentidad,
                EsActivo = curso.EsActivo
            };
        }

        public async Task<CursoDocenteObtenerDto> ActivarAsync(CursoIdDto dto)
        {
            var curso = await _context.Cursos
                .Include(c => c.Docente)
                .FirstOrDefaultAsync(c => c.Id == dto.Id);

            if (curso == null)
                throw new Exception("Curso no encontrado");

            if (curso.EsActivo)
                throw new Exception("El curso ya está activo");

            curso.EsActivo = true;

            await _context.SaveChangesAsync();

            return new CursoDocenteObtenerDto
            {
                Id = curso.Id,
                Nombre = curso.Nombre,
                Descripcion = curso.Descripcion,
                DocenteId = curso.DocenteId,
                Nombres = curso.Docente?.Nombres ?? "Desconocido",
                Apellidos = curso.Docente?.Apellidos ?? "Desconocido",
                CarnetIdentidad = curso.Docente?.CarnetIdentidad ?? 0,
                EsActivo = curso.EsActivo
            };
        }

        public async Task<CursoDocenteObtenerDto> EliminarLogicoAsync(CursoIdDto dto)
        {
            var curso = await _context.Cursos
                .Include(c => c.Docente)
                .FirstOrDefaultAsync(c => c.Id == dto.Id);

            if (curso == null)
                throw new Exception("Curso no encontrado");

            curso.EsActivo = false;

            await _context.SaveChangesAsync();

            return new CursoDocenteObtenerDto
            {
                Id = curso.Id,
                Nombre = curso.Nombre,
                Descripcion = curso.Descripcion,
                DocenteId = curso.DocenteId,
                Nombres = curso.Docente?.Nombres ?? "Desconocido",
                Apellidos = curso.Docente?.Apellidos ?? "Desconocido",
                CarnetIdentidad = curso.Docente?.CarnetIdentidad ?? 0,
                EsActivo = curso.EsActivo
            };
        }
    }
}
