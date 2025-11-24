using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Rol> Roles => Set<Rol>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Curso> Cursos => Set<Curso>();
        public DbSet<Inscripcion> Inscripciones => Set<Inscripcion>();
        public DbSet<Material> Materiales => Set<Material>();
        public DbSet<Evaluacion> Evaluaciones => Set<Evaluacion>();
        public DbSet<ResultadoEvaluacion> ResultadosEvaluaciones => Set<ResultadoEvaluacion>();
        public DbSet<Mensaje> Mensajes => Set<Mensaje>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
