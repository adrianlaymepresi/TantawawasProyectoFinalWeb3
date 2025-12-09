using backend.Data;
using backend.Models;
using backend.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .Select(u => new UsuarioObtenerDto
                {
                    Id = u.Id,
                    Nombres = u.Nombres,
                    Apellidos = u.Apellidos,
                    CarnetIdentidad = u.CarnetIdentidad,
                    Email = u.Email,
                    RolId = u.RolId,
                    NombreRol = u.Rol != null ? u.Rol.NombreRol : "UsuarioMisterioso",
                    EsUsuarioActivo = u.EsUsuarioActivo
                })
                .ToListAsync();

            return usuario;
        }

        public async Task<UsuarioObtenerDto> ObtenerPorIdAsync(UsuarioIdDto dto)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.Id == dto.Id)
                .Select(u => new UsuarioObtenerDto
                {
                    Id = u.Id,
                    Nombres = u.Nombres,
                    Apellidos = u.Apellidos,
                    CarnetIdentidad = u.CarnetIdentidad,
                    Email = u.Email,
                    RolId = u.RolId,
                    NombreRol = u.Rol != null ? u.Rol.NombreRol : "UsuarioMisterioso",
                    EsUsuarioActivo = u.EsUsuarioActivo
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            return usuario;
        }

        public async Task<List<UsuarioObtenerDto>> BuscarPorCarnetIdentidadAsync(UsuarioBuscarPorCI dto)
        {
            string ci = dto.CarnetIdentidad.Trim();

            var usuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.CarnetIdentidad.ToString().Contains(ci))
                .Select(u => new UsuarioObtenerDto
                {
                    Id = u.Id,
                    Nombres = u.Nombres,
                    Apellidos = u.Apellidos,
                    CarnetIdentidad = u.CarnetIdentidad,
                    Email = u.Email,
                    RolId = u.RolId,
                    NombreRol = u.Rol != null ? u.Rol.NombreRol : "UsuarioMisterioso",
                    EsUsuarioActivo = u.EsUsuarioActivo
                })
                .ToListAsync();

            if (!usuarios.Any())
                throw new Exception("No se encontraron usuarios con el CI proporcionado");

            return usuarios;
        }

        public async Task<List<UsuarioObtenerDto>> BuscarPorNombreCompletoAsync(UsuarioBuscarPorNombreCompleto dto)
        {
            string nombre = dto.NombreCompleto.Trim().ToLower();

            var usuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .Where(u => (u.Nombres + " " + u.Apellidos).ToLower().Contains(nombre))
                .Select(u => new UsuarioObtenerDto
                {
                    Id = u.Id,
                    Nombres = u.Nombres,
                    Apellidos = u.Apellidos,
                    CarnetIdentidad = u.CarnetIdentidad,
                    Email = u.Email,
                    RolId = u.RolId,
                    NombreRol = u.Rol != null ? u.Rol.NombreRol : "UsuarioMisterioso",
                    EsUsuarioActivo = u.EsUsuarioActivo
                })
                .ToListAsync();

            if (!usuarios.Any())
                throw new Exception("No se encontraron usuarios con ese nombre");

            return usuarios;
        }

        public async Task<UsuarioObtenerDto> CrearAsync(UsuarioCrearDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("El email ya está registrado");

            if (await _context.Usuarios.AnyAsync(u => u.CarnetIdentidad == dto.CarnetIdentidad))
                throw new Exception("El carnet de identidad ya está registrado");

            if (dto.CarnetIdentidad < 1_000_000 || dto.CarnetIdentidad > 999_999_999)
                throw new Exception("El carnet debe tener entre 7 y 9 dígitos.");

            var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Id == dto.RolId);
            if (rol == null)
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

            return new UsuarioObtenerDto
            {
                Id = usuario.Id,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                CarnetIdentidad = usuario.CarnetIdentidad,
                Email = usuario.Email,
                RolId = usuario.RolId,
                NombreRol = rol.NombreRol,
                EsUsuarioActivo = usuario.EsUsuarioActivo
            };
        }

        public async Task<UsuarioObtenerDto> ActualizarAsync(UsuarioActualizarDto dto)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == dto.Id);

            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            if (await _context.Usuarios.AnyAsync(u =>
                u.Email == dto.Email && u.Id != dto.Id))
                throw new Exception("El email ya está registrado por otro usuario");

            if (await _context.Usuarios.AnyAsync(u =>
                u.CarnetIdentidad == dto.CarnetIdentidad && u.Id != dto.Id))
                throw new Exception("El carnet de identidad ya está registrado por otro usuario");

            if (dto.CarnetIdentidad < 1_000_000 || dto.CarnetIdentidad > 999_999_999)
                throw new Exception("El carnet debe tener entre 7 y 9 dígitos.");

            var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Id == dto.RolId);
            if (rol == null)
                throw new Exception("El rol especificado no existe");

            usuario.Nombres = dto.Nombres;
            usuario.Apellidos = dto.Apellidos;
            usuario.CarnetIdentidad = dto.CarnetIdentidad;
            usuario.Email = dto.Email;
            usuario.RolId = dto.RolId;

            await _context.SaveChangesAsync();

            return new UsuarioObtenerDto
            {
                Id = usuario.Id,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                CarnetIdentidad = usuario.CarnetIdentidad,
                Email = usuario.Email,
                RolId = usuario.RolId,
                NombreRol = rol.NombreRol,
                EsUsuarioActivo = usuario.EsUsuarioActivo
            };
        }

        public async Task CambiarPasswordAsync(UsuarioCambiarPasswordDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.Id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            if (usuario.Password != HashPassword(dto.PasswordActual))
                throw new Exception("La contraseña actual no es correcta");

            usuario.Password = HashPassword(dto.NuevaPassword);
            await _context.SaveChangesAsync();
        }

        public async Task ReestablecerPasswordAsync(UsuarioResetPasswordDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(dto.Id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            usuario.Password = HashPassword(dto.NuevaPassword);
            await _context.SaveChangesAsync();
        }

        public async Task<UsuarioObtenerDto> ActivarAsync(UsuarioIdDto dto)
        {
            var usuario = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Id == dto.Id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            usuario.EsUsuarioActivo = true;
            await _context.SaveChangesAsync();

            return new UsuarioObtenerDto
            {
                Id = usuario.Id,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                CarnetIdentidad = usuario.CarnetIdentidad,
                Email = usuario.Email,
                RolId = usuario.RolId,
                NombreRol = usuario.Rol?.NombreRol ?? "",
                EsUsuarioActivo = usuario.EsUsuarioActivo
            };
        }

        public async Task<UsuarioObtenerDto> EliminarLogicoAsync(UsuarioIdDto dto)
        {
            var idClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (idClaim == null)
                throw new Exception("No se pudo validar el usuario actual.");

            int idUsuarioActual = int.Parse(idClaim);

            if (dto.Id == idUsuarioActual)
                throw new Exception("No puedes desactivar tu propia cuenta.");

            var usuario = await _context.Usuarios.Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == dto.Id);

            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            usuario.EsUsuarioActivo = false;
            await _context.SaveChangesAsync();

            return new UsuarioObtenerDto
            {
                Id = usuario.Id,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                CarnetIdentidad = usuario.CarnetIdentidad,
                Email = usuario.Email,
                RolId = usuario.RolId,
                NombreRol = usuario.Rol?.NombreRol ?? "",
                EsUsuarioActivo = usuario.EsUsuarioActivo
            };
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
