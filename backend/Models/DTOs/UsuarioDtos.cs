using System.ComponentModel.DataAnnotations;

namespace backend.Models.DTOs
{
    public class UsuarioCrearDto
    {
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

        [Required]
        public int RolId { get; set; }
    }

    public class UsuarioActualizarDto
    {
        [Required]
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
        public int RolId { get; set; }
    }

    public class UsuarioCambiarPasswordDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string PasswordActual { get; set; } = string.Empty;

        [Required]
        public string NuevaPassword { get; set; } = string.Empty;
    }

    public class UsuarioResetPasswordDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string NuevaPassword { get; set; } = string.Empty;
    }

    public class UsuarioIdDto
    {
        [Required]
        public int Id { get; set; }
    }

    public class UsuarioBuscarPorCI
    {
        [Required]
        public string CarnetIdentidad { get; set; } = string.Empty;
    }

    public class UsuarioBuscarPorNombreCompleto
    {
        [Required]
        public string NombreCompleto { get; set; } = string.Empty;
    }

    public class UsuarioLoginDto
    {
        [Required]
        [Range(1_000_000, 999_999_999)]
        public int CarnetIdentidad { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class UsuarioObtenerDto
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nombres { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Apellidos { get; set; } = string.Empty;

        [Required]
        [Range(1_000_000, 999_999_999)]
        public int CarnetIdentidad { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public int RolId { get; set; }

        [Required, MaxLength(50)]
        public string NombreRol { get; set; } = string.Empty;

        [Required]
        public bool EsUsuarioActivo { get; set; }
    }
}
