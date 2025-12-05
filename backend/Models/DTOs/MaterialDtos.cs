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
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(150)]
        public string? Titulo { get; set; } = string.Empty;
        public byte[]? ArchivoAdjunto { get; set; }
    }

    public class MaterialIdDto
    {
        public int Id { get; set; }
    }

    public class MaterialObtenerDto
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Titulo { get; set; } = string.Empty;

        public byte[]? ArchivoAdjunto { get; set; }

        public DateTime? FechaCreacion { get; set; }

        [Required]
        public int CursoId { get; set; }

        [Required, MaxLength(150)]
        public string CursoNombre { get; set; } = string.Empty;
    }

    public class MaterialSimpleDto
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public DateTime? FechaCreacion { get; set; }
    }
}
