using System.ComponentModel.DataAnnotations;

namespace backend.Models.DTOs
{
    public class MaterialCrearDto
    {
        [Required, MaxLength(150)]
        public string Titulo { get; set; } = string.Empty;

        public byte[]? ArchivoAdjunto { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required]
        public int CursoId { get; set; }
    }

    public class MaterialActualizarDto
    {
        public int Id { get; set; }
        [Required, MaxLength(150)]
        public string? Titulo { get; set; } = string.Empty;
        public byte[]? ArchivoAdjunto { get; set; }
    }

    public class MaterialIdDto
    {
        public int Id { get; set; }
    }
}
