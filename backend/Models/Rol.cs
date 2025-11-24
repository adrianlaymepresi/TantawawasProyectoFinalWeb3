using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Rol
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string NombreRol { get; set; } = string.Empty;

        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
