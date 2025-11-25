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
}
