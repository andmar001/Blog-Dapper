using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace BlogDapper.Models
{
    public class Slider
    {
        [Key]
        public int IdSlider { get; set; }

        [Required(ErrorMessage ="El nombre del slider es obligatorio")]
        public string Nombre { get; set; }
        public string UrlImagen { get; set; }

    }
}
