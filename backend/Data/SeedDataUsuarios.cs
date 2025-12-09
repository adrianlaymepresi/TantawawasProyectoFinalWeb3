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

            if (!context.Cursos.Any())
            {
                var docente = context.Usuarios.First(u =>
                    u.RolId == context.Roles.First(r => r.NombreRol == "Docente").Id
                );

                context.Cursos.AddRange(new List<Curso>
                {
                    new Curso { Nombre = "Programacion Web III", Descripcion = "Curso avanzado de desarrollo web", DocenteId = docente.Id, EsActivo = true },
                    new Curso { Nombre = "Programacion Movil II", Descripcion = "Curso de desarrollo móvil con Android", DocenteId = docente.Id, EsActivo = true },
                    new Curso { Nombre = "Ingenieria de Software", Descripcion = "Metodologías y arquitectura de software", DocenteId = docente.Id, EsActivo = true }
                });

                await context.SaveChangesAsync();
            }

            if (!context.Inscripciones.Any())
            {
                var estudiante = context.Usuarios.First(u =>
                    u.RolId == context.Roles.First(r => r.NombreRol == "Estudiante").Id
                );

                var cursos = context.Cursos.Take(3).ToList();

                context.Inscripciones.AddRange(new List<Inscripcion>
                {
                    new Inscripcion { EstudianteId = estudiante.Id, CursoId = cursos[2].Id, FechaInscripcion = DateTime.Now.AddMonths(-3) },
                    new Inscripcion { EstudianteId = estudiante.Id, CursoId = cursos[1].Id, FechaInscripcion = DateTime.Now.AddMonths(-3) }
                });

                await context.SaveChangesAsync();
            }

            if (!context.Materiales.Any())
            {
                var cursos = context.Cursos.Take(3).ToList();

                context.Materiales.AddRange(new List<Material>
                {
                    new Material 
                    { 
                        Titulo = "Introducción a ASP.NET Core",
                        ArchivoAdjunto = null,
                        FechaCreacion = DateTime.Now.AddMonths(-2),
                        CursoId = cursos[2].Id
                    },
                    new Material 
                    { 
                        Titulo = "Entity Framework Core - Guía Práctica",
                        ArchivoAdjunto = null,
                        FechaCreacion = DateTime.Now.AddMonths(-2).AddDays(5),
                        CursoId = cursos[2].Id
                    },
                    new Material 
                    { 
                        Titulo = "APIs RESTful con .NET",
                        ArchivoAdjunto = null,
                        FechaCreacion = DateTime.Now.AddMonths(-1),
                        CursoId = cursos[2].Id
                    },
                    new Material 
                    { 
                        Titulo = "Autenticación y Autorización JWT",
                        ArchivoAdjunto = null,
                        FechaCreacion = DateTime.Now.AddDays(-15),
                        CursoId = cursos[2].Id
                    },

                    new Material 
                    { 
                        Titulo = "Introducción a Android Jetpack",
                        ArchivoAdjunto = null,
                        FechaCreacion = DateTime.Now.AddMonths(-2),
                        CursoId = cursos[1].Id
                    },
                    new Material 
                    { 
                        Titulo = "Arquitectura MVVM en Android",
                        ArchivoAdjunto = null,
                        FechaCreacion = DateTime.Now.AddMonths(-1).AddDays(10),
                        CursoId = cursos[1].Id
                    },
                    new Material 
                    { 
                        Titulo = "Room Database - Persistencia Local",
                        ArchivoAdjunto = null,
                        FechaCreacion = DateTime.Now.AddDays(-20),
                        CursoId = cursos[1].Id
                    }
                });

                await context.SaveChangesAsync();
            }

            if (!context.Mensajes.Any())
            {
                var docente = context.Usuarios.First(u =>
                    u.RolId == context.Roles.First(r => r.NombreRol == "Docente").Id
                );
                var cursos = context.Cursos.Take(3).ToList();

                context.Mensajes.AddRange(new List<Mensaje>
                {
                    new Mensaje
                    {
                        Contenido = "Bienvenidos al curso de Programación Web III.",
                        ArchivoAdjunto = null,
                        FechaEnvio = DateTime.Now.AddMonths(-2).AddDays(-5),
                        CursoId = cursos[2].Id,
                        UsuarioId = docente.Id
                    },
                    new Mensaje
                    {
                        Contenido = "Recordatorio: La primera evaluación será el próximo viernes.",
                        ArchivoAdjunto = null,
                        FechaEnvio = DateTime.Now.AddMonths(-1).AddDays(-3),
                        CursoId = cursos[2].Id,
                        UsuarioId = docente.Id
                    },
                    new Mensaje
                    {
                        Contenido = "Se ha subido nuevo material sobre Autenticación. Por favor revisenlo antes de la próxima clase.",
                        ArchivoAdjunto = null,
                        FechaEnvio = DateTime.Now.AddDays(-14),
                        CursoId = cursos[2].Id,
                        UsuarioId = docente.Id
                    },
                    new Mensaje
                    {
                        Contenido = "Importante: El proyecto final debe ser entregado antes del 10 de diciembre. Consulten las especificaciones en el material del curso.",
                        ArchivoAdjunto = null,
                        FechaEnvio = DateTime.Now.AddDays(-5),
                        CursoId = cursos[2].Id,
                        UsuarioId = docente.Id
                    },

                    new Mensaje
                    {
                        Contenido = "Bienvenidos a Programación Móvil II. Este semestre nos enfocaremos en desarrollo Android avanzado con sensores y Jetpack.",
                        ArchivoAdjunto = null,
                        FechaEnvio = DateTime.Now.AddMonths(-2).AddDays(-5),
                        CursoId = cursos[1].Id,
                        UsuarioId = docente.Id
                    },
                    new Mensaje
                    {
                        Contenido = "Les he compartido material sobre arquitectura MVVM. Es fundamental para el desarrollo del proyecto.",
                        ArchivoAdjunto = null,
                        FechaEnvio = DateTime.Now.AddMonths(-1).AddDays(-5),
                        CursoId = cursos[1].Id,
                        UsuarioId = docente.Id
                    },
                    new Mensaje
                    {
                        Contenido = "La clase del martes será práctica. Traigan sus laptops con Android Studio instalado.",
                        ArchivoAdjunto = null,
                        FechaEnvio = DateTime.Now.AddDays(-7),
                        CursoId = cursos[1].Id,
                        UsuarioId = docente.Id
                    }
                });

                await context.SaveChangesAsync();
            }

            // Agregar evaluaciones de prueba
            if (!context.Evaluaciones.Any())
            {
                var cursos = context.Cursos.Take(3).ToList();

                context.Evaluaciones.AddRange(new List<Evaluacion>
                {
                    new Evaluacion
                    {
                        Titulo = "Examen Parcial 1",
                        Descripcion = "Evaluación sobre fundamentos de ASP.NET Core y Entity Framework",
                        FechaCreacion = DateTime.Now.AddMonths(-1).AddDays(-10),
                        CursoId = cursos[2].Id
                    },
                    new Evaluacion
                    {
                        Titulo = "Práctica - API",
                        Descripcion = "Desarrollo de una API REST completa con operaciones CRUD",
                        FechaCreacion = DateTime.Now.AddDays(-25),
                        CursoId = cursos[2].Id
                    },
                    new Evaluacion
                    {
                        Titulo = "Examen Parcial 2",
                        Descripcion = "Evaluación sobre autenticación, autorización en APIs",
                        FechaCreacion = DateTime.Now.AddDays(-10),
                        CursoId = cursos[2].Id
                    },

                    new Evaluacion
                    {
                        Titulo = "Examen Parcial 1",
                        Descripcion = "Evaluación sobre componentes de Jetpack y arquitectura MVVM",
                        FechaCreacion = DateTime.Now.AddMonths(-1).AddDays(-8),
                        CursoId = cursos[1].Id
                    },
                    new Evaluacion
                    {
                        Titulo = "Proyecto - App con Flutter",
                        Descripcion = "Desarrollo de una aplicación Android con widgets y POO",
                        FechaCreacion = DateTime.Now.AddDays(-18),
                        CursoId = cursos[1].Id
                    }
                });

                await context.SaveChangesAsync();
            }

            if (!context.ResultadosEvaluaciones.Any())
            {
                var estudiante = context.Usuarios.First(u =>
                    u.RolId == context.Roles.First(r => r.NombreRol == "Estudiante").Id
                );
                var evaluaciones = context.Evaluaciones.ToList();

                context.ResultadosEvaluaciones.AddRange(new List<ResultadoEvaluacion>
                {
                    new ResultadoEvaluacion
                    {
                        EstudianteId = estudiante.Id,
                        EvaluacionId = evaluaciones[4].Id,
                        Nota = 85.5m
                    },
                    new ResultadoEvaluacion
                    {
                        EstudianteId = estudiante.Id,
                        EvaluacionId = evaluaciones[3].Id,
                        Nota = 92.0m
                    },
                    new ResultadoEvaluacion
                    {
                        EstudianteId = estudiante.Id,
                        EvaluacionId = evaluaciones[2].Id,
                        Nota = 78.5m
                    },

                    new ResultadoEvaluacion
                    {
                        EstudianteId = estudiante.Id,
                        EvaluacionId = evaluaciones[1].Id,
                        Nota = 88.0m
                    },
                    new ResultadoEvaluacion
                    {
                        EstudianteId = estudiante.Id,
                        EvaluacionId = evaluaciones[0].Id,
                        Nota = 95.0m
                    }
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
