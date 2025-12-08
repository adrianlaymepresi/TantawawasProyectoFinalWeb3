using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nombres { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Apellidos { get; set; } = string.Empty;

        [Required]
        [Range(1_000_000, 999_999_999, ErrorMessage = "El carnet debe tener entre 7 y 9 dígitos.")]
        public int CarnetIdentidad { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public bool EsUsuarioActivo { get; set; } = true;

        [Required]
        public int RolId { get; set; }
        public Rol? Rol { get; set; } 
    }
}
