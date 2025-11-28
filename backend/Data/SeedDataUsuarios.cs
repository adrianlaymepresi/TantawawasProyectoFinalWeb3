using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace backend.Data
{
    public static class SeedDataUsuarios
    {
        public static async Task Inicializar(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();

            context.Database.EnsureCreated();

            if (!context.Roles.Any())
            {
                context.Roles.AddRange(new List<Rol>
                {
                    new Rol { NombreRol = "Administrador" },
                    new Rol { NombreRol = "Docente" },
                    new Rol { NombreRol = "Estudiante" }
                });

                await context.SaveChangesAsync();
            }

            if (!context.Usuarios.Any())
            {
                var admin = new Usuario
                {
                    Nombres = "Admin",
                    Apellidos = "Master",
                    CarnetIdentidad = 12345670,
                    Email = "admin@test.com",
                    Password = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes("admin123"))),
                    EsUsuarioActivo = true,
                    RolId = context.Roles.First(r => r.NombreRol == "Administrador").Id
                };

                var docente = new Usuario
                {
                    Nombres = "Docente",
                    Apellidos = "Profesor",
                    CarnetIdentidad = 12345671,
                    Email = "docente@test.com",
                    Password = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes("docente123"))),
                    EsUsuarioActivo = true,
                    RolId = context.Roles.First(r => r.NombreRol == "Docente").Id
                };

                var estudiante = new Usuario
                {
                    Nombres = "Alumno",
                    Apellidos = "Estudiante",
                    CarnetIdentidad = 12345672,
                    Email = "alumno@test.com",
                    Password = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes("alumno123"))),
                    EsUsuarioActivo = true,
                    RolId = context.Roles.First(r => r.NombreRol == "Estudiante").Id
                };

                context.Usuarios.AddRange(admin, docente, estudiante);
                await context.SaveChangesAsync();
            }
        }
    }
}
