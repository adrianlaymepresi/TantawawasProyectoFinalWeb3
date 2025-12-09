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

    public class EvaluacionObtenerDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public int CursoId { get; set; }
        public string CursoNombre { get; set; } = string.Empty;
        public int CantidadResultados { get; set; }
        public decimal PromedioNotas { get; set; }
    }

    public class EvaluacionSimpleDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public int CantidadResultados { get; set; }
        public decimal PromedioNotas { get; set; }
    }
}
