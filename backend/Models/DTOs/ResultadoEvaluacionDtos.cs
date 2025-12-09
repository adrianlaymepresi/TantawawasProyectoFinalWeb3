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

    public class ResultadoEvaluacionObtenerDto
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public string EstudianteNombreCompleto { get; set; } = string.Empty;
        public int EvaluacionId { get; set; }
        public string EvaluacionTitulo { get; set; } = string.Empty;
        public int CursoId { get; set; }
        public string CursoNombre { get; set; } = string.Empty;
        public decimal Nota { get; set; }
    }

    public class ResultadoEvaluacionSimpleDto
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public string EstudianteNombreCompleto { get; set; } = string.Empty;
        public decimal Nota { get; set; }
    }

    public class ResultadoPorEstudianteDto
    {
        public int Id { get; set; }
        public int EvaluacionId { get; set; }
        public string EvaluacionTitulo { get; set; } = string.Empty;
        public int CursoId { get; set; }
        public string CursoNombre { get; set; } = string.Empty;
        public DateTime FechaEvaluacion { get; set; }
        public decimal Nota { get; set; }
    }

    public class ResultadoPorCursoEstudianteDto
    {
        public int Id { get; set; }
        public int EvaluacionId { get; set; }
        public string EvaluacionTitulo { get; set; } = string.Empty;
        public DateTime FechaEvaluacion { get; set; }
        public decimal Nota { get; set; }
    }
}
