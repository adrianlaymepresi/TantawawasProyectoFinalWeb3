using System.ComponentModel.DataAnnotations;

namespace backend.Models.DTOs
{
    public class RolCrearDto
    {
        [Required, MaxLength(50)]
        public string NombreRol { get; set; } = string.Empty;
    }

    public class RolActualizarDto
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string NombreRol { get; set; } = string.Empty;
    }

    public class RolIdDto
    {
        [Required]
        public int Id { get; set; }
    }
}
