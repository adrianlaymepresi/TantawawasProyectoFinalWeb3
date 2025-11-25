using System.ComponentModel.DataAnnotations;

namespace backend.Models.DTOs
{
    public class EvaluacionCrearDto
    {
        [Required, MaxLength(120)]
        public string Titulo { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Descripcion { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required]
        public int CursoId { get; set; }
    }

    public class EvaluacionActualizarDto
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(120)]
        public string Titulo { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Descripcion { get; set; } = string.Empty;
    }

    public class EvaluacionIdDto
    {
        public int Id { get; set; }
    }
}
