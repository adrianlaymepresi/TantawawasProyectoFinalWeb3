using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Mensaje
    {
        public int Id { get; set; }

        [Required, MaxLength(350)]
        public string Contenido { get; set; } = string.Empty;

        public DateTime FechaEnvio { get; set; } = DateTime.Now;

        public byte[]? ArchivoAdjunto { get; set; }

        [Required]
        public int CursoId { get; set; }
        public Curso? Curso { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
