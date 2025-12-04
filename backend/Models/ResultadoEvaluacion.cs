using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class ResultadoEvaluacion
    {
        public int Id { get; set; }

        [Required]
        public int EstudianteId { get; set; }
        public Usuario? Estudiante { get; set; }

        [Required]
        public int EvaluacionId { get; set; }
        public Evaluacion? Evaluacion { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "La nota debe estar entre 0 y 100")]
        public decimal Nota { get; set; }
    }
}
