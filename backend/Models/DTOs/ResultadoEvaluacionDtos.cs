using System.ComponentModel.DataAnnotations;

namespace backend.Models.DTOs
{
    public class ResultadoEvaluacionCrearDto
    {
        [Required]
        public int EstudianteId { get; set; }

        [Required]
        public int EvaluacionId { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "La nota debe estar entre 0 y 100")]
        public decimal Nota { get; set; }
    }

    public class ResultadoEvaluacionActualizarDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "La nota debe estar entre 0 y 100")]
        public decimal Nota { get; set; }
    }

    public class ResultadoEvaluacionIdDto
    {
        public int Id { get; set; }
    }
}
