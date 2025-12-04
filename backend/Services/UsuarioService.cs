using backend.Data;
using backend.Models;
using backend.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace backend.Services
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioService(AppDbContext context, JwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<UsuarioObtenerDto>> ObtenerTodosAsync()
        {
            return await _context.Usuarios.Include(u => u.Rol)
                .Select(u => new UsuarioObtenerDto
                {
                    Id = u.Id,
                    Nombres = u.Nombres,
                    Apellidos = u.Apellidos,
                    CarnetIdentidad = u.CarnetIdentidad,
                    Email = u.Email,
                    RolId = u.RolId,
                    NombreRol = u.Rol != null ? u.Rol.NombreRol : null
                })
                .ToListAsync();
        }

        public async Task<Usuario> ObtenerPorIdAsync(UsuarioIdDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.Id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            return usuario;
        }

        public async Task<List<Usuario>> BuscarPorCarnetIdentidadAsync(UsuarioBuscarPorCI dto)
        {
            string ci = dto.CarnetIdentidad.Trim();

            var usuarios = await _context.Usuarios
                .Where(u => u.CarnetIdentidad.ToString().Contains(ci))
                .ToListAsync();

            if (!usuarios.Any())
                throw new Exception("No se encontraron usuarios con el CI proporcionado");

            return usuarios;
        }

        public async Task<List<Usuario>> BuscarPorNombreCompletoAsync(UsuarioBuscarPorNombreCompleto dto)
        {
            string nombre = dto.NombreCompleto.Trim().ToLower();

            var usuarios = await _context.Usuarios
                .Where(u =>
                    (u.Nombres + " " + u.Apellidos).ToLower().Contains(nombre))
                .ToListAsync();

            if (!usuarios.Any())
                throw new Exception("No se encontraron usuarios con ese nombre");

            return usuarios;
        }

        public async Task<Usuario> CrearAsync(UsuarioCrearDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("El email ya está registrado");

            if (await _context.Usuarios.AnyAsync(u => u.CarnetIdentidad == dto.CarnetIdentidad))
                throw new Exception("El carnet de identidad ya está registrado");

            var rolExiste = await _context.Roles.AnyAsync(r => r.Id == dto.RolId);
            if (!rolExiste)
                throw new Exception("El rol especificado no existe");

            var usuario = new Usuario
            {
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                CarnetIdentidad = dto.CarnetIdentidad,
                Email = dto.Email,
                RolId = dto.RolId,
                Password = HashPassword(dto.Password),
                EsUsuarioActivo = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> ActualizarAsync(UsuarioActualizarDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.Id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            if (await _context.Usuarios.AnyAsync(u => u.CarnetIdentidad == dto.CarnetIdentidad && u.Id != dto.Id))
                throw new Exception("El carnet de identidad ya está registrado por otro usuario");

            var rolExiste = await _context.Roles.AnyAsync(r => r.Id == dto.RolId);
            if (!rolExiste)
                throw new Exception("El rol especificado no existe");

            usuario.Nombres = dto.Nombres;
            usuario.Apellidos = dto.Apellidos;
            usuario.CarnetIdentidad = dto.CarnetIdentidad;
            usuario.Email = dto.Email;
            usuario.RolId = dto.RolId;

            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> ActualizarPasswordAsync(UsuarioActualizarPasswordDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.Id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            usuario.Password = HashPassword(dto.NuevoPassword);

            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> EliminarLogicoAsync(UsuarioIdDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.Id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            usuario.EsUsuarioActivo = false;
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> EliminarFisicoAsync(UsuarioIdDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.Id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<string> LoginAsync(UsuarioLoginDto dto)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.CarnetIdentidad == dto.CarnetIdentidad);

            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            if (usuario.Password != HashPassword(dto.Password))
                throw new Exception("Contraseña incorrecta");

            if (!usuario.EsUsuarioActivo)
                throw new Exception("El usuario está desactivado");

            var token = _jwtService.GenerarToken(usuario);

            // Establecer la cookie HttpOnly con el token
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMinutes(120)
            };

            _httpContextAccessor.HttpContext?.Response.Cookies.Append("AuthToken", token, cookieOptions);

            return token;
        }

        public void Logout()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("AuthToken");
        }

        private string HashPassword(string password)
        {
            var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
