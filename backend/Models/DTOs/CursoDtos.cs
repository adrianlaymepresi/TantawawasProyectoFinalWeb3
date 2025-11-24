using System.ComponentModel.DataAnnotations;

namespace backend.Models.DTOs
{
    public class CursoCrearDto
    {
        [Required, MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(300)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public int DocenteId { get; set; }
    }

    public class CursoActualizarDto
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(300)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public int DocenteId { get; set; }
    }

    public class CursoIdDto
    {
        [Required]
        public int Id { get; set; }
    }

    public class CursoBuscarPorNombreDto
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;
    }
}
