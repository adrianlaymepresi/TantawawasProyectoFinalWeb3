using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Evaluacion
    {
        public int Id { get; set; }

        [Required, MaxLength(120)]
        public string Titulo { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Descripcion { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required]
        public int CursoId { get; set; }
        public Curso? Curso { get; set; }

        public List<ResultadoEvaluacion> Resultados { get; set; } = new List<ResultadoEvaluacion>();
    }
}
