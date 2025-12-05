using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Material
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Titulo { get; set; } = string.Empty;

        public byte[]? ArchivoAdjunto { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required]
        public int CursoId { get; set; }
        public Curso? Curso { get; set; }
    }
}
