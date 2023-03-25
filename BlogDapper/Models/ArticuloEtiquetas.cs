using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace BlogDapper.Models
{
    public class ArticuloEtiquetas
    {
        [Key]
        public int IdArticulo { get; set; }
        [Key]
        public int IdEtiqueta { get; set; }
    }
}
