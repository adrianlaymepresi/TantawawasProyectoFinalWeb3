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
}
