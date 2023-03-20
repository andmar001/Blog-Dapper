using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace BlogDapper.Models
{
    public class Etiqueta
    {
        [Key]
        public int IdEtiqueta { get; set; }

        [Required(ErrorMessage ="El nombre de la etiqueta es obligatorio")]
        public string NombreEtiqueta { get; set; }
        public DateTime FechaCreacion { get; set; }   
        
        //Esta indica la relación con articulo, con una tabla intermedia ArticuloEtiqueta
        public List<Articulo> Articulo { get; set; }

    }
}
