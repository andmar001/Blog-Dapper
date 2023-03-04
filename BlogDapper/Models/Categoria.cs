using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace BlogDapper.Models
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage ="El nombre de categoría es obligatorio")]
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }     

    }
}
