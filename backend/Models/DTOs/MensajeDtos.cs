using System.ComponentModel.DataAnnotations;

namespace backend.Models.DTOs
{
    public class MensajeCrearDto
    {
        [Required, MaxLength(350)]
        public string Contenido { get; set; } = string.Empty;

        public byte[]? ArchivoAdjunto { get; set; }

        public DateTime FechaEnvio { get; set; } = DateTime.Now;

        [Required]
        public int CursoId { get; set; }

        [Required]
        public int UsuarioId { get; set; }
    }

    public class MensajeActualizarDto
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(350)]
        public string Contenido { get; set; } = string.Empty;

        public byte[]? ArchivoAdjunto { get; set; }
    }

    public class MensajeIdDto
    {
        public int Id { get; set; }
    }

    public class MensajeObtenerDto
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(350)]
        public string Contenido { get; set; } = string.Empty;

        public byte[]? ArchivoAdjunto { get; set; }

        public DateTime FechaEnvio { get; set; }

        [Required]
        public int CursoId { get; set; }

        [Required, MaxLength(150)]
        public string CursoNombre { get; set; } = string.Empty;

        [Required]
        public int UsuarioId { get; set; }

        [Required, MaxLength(200)]
        public string UsuarioNombreCompleto { get; set; } = string.Empty;
    }

    public class MensajeSimpleDto
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(350)]
        public string Contenido { get; set; } = string.Empty;

        public DateTime FechaEnvio { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required, MaxLength(200)]
        public string UsuarioNombreCompleto { get; set; } = string.Empty;
    }

    public class MensajePorUsuarioDto
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(350)]
        public string Contenido { get; set; } = string.Empty;

        public DateTime FechaEnvio { get; set; }

        [Required]
        public int CursoId { get; set; }

        [Required, MaxLength(150)]
        public string CursoNombre { get; set; } = string.Empty;
    }
}
