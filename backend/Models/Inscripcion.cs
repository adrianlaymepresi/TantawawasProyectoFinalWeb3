using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Inscripcion
    {
        public int Id { get; set; }

        [Required]
        public int EstudianteId { get; set; }
        public Usuario? Estudiante { get; set; }

        [Required]
        public int CursoId { get; set; }
        public Curso? Curso { get; set; }

        public DateTime FechaInscripcion { get; set; } = DateTime.Now;
    }
}
