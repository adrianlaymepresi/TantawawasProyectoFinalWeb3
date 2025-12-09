using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Curso
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(300)]
        public string Descripcion { get; set; } = string.Empty;

        public bool EsActivo { get; set; } = true;

        [Required]
        public int DocenteId { get; set; }
        public Usuario? Docente { get; set; }

        public List<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
        public List<Material> Materiales { get; set; } = new List<Material>();
        public List<Evaluacion> Evaluaciones { get; set; } = new List<Evaluacion>();
        public List<Mensaje> Mensajes { get; set; } = new List<Mensaje>();
    }
}
