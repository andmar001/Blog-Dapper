
using System.ComponentModel.DataAnnotations;

namespace BlogDapper.Models
{
    public class Comentario
    {
        [Key]
        public int IdComentario { get; set; }
        [Required(ErrorMessage = "El Título es obligatorio")]
        public string Titulo { get; set; }
        [Required]
        [StringLength(300, MinimumLength =10, ErrorMessage = "El mensaje debe tener como minimo 10 caracteres y máximo como 300")]
        public string Mensaje { get; set; }
        public DateTime FechaCreacion { get; set; }
        //Llave foranea
        //[Required(ErrorMessage ="El nombre de Categoría es obligatorio")] 
        public int ArticuloId { get; set; }
        //Relacion con categoría - un articulo debe pertenecer a una sola categoría
        public virtual Articulo Articulo { get; set;}
    }
}
