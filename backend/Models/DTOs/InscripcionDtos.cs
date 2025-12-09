using System.ComponentModel.DataAnnotations;

namespace backend.Models.DTOs
{
    public class InscripcionCrearDto
    {
        [Required]
        public int EstudianteId { get; set; }

        [Required]
        public int CursoId { get; set; }
    }

    public class InscripcionActualizarDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int EstudianteId { get; set; }

        [Required]
        public int CursoId { get; set; }
    }

    public class InscripcionIdDto
    {
        [Required]
        public int Id { get; set; }
    }

    public class InscripcionBuscarPorEstudianteDto
    {
        [Required]
        public int EstudianteId { get; set; }
    }

    public class InscripcionBuscarPorCursoDto
    {
        [Required]
        public int CursoId { get; set; }
    }

    public class InscripcionObtenerDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int EstudianteId { get; set; }

        [Required, MaxLength(100)]
        public string NombresEstudiante { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string ApellidosEstudiante { get; set; } = string.Empty;

        [Required]
        public int CursoId { get; set; }

        [Required, MaxLength(150)]
        public string NombreCurso { get; set; } = string.Empty;

        [Required]
        public string FechaInscripcion { get; set; }
    }
}
