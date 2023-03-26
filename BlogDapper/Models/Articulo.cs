
using System.ComponentModel.DataAnnotations;

namespace BlogDapper.Models
{
    public class Articulo
    {
        //soporte para lista de etiquetas
        public Articulo() 
        {
            Etiqueta = new List<Etiqueta>();
        }
        [Key]
        public int IdArticulo { get; set; }
        [Required(ErrorMessage = "El Título es obligatorio")] //vendra de un dropdown
        public string Titulo { get; set; }
        [Required]
        [StringLength(1000, MinimumLength =10, ErrorMessage = "La descripción debe tener como minimo 10 caracteres y máximo como 1000")]
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }

        //mapeo de llave foránea
        [Required(ErrorMessage ="El nombre de Categoría es obligatorio")] //vendra de un dropdown
        public int CategoriaId { get; set; }
        //Relacion con categoría - un articulo debe pertenecer a una sola categoría
        public virtual Categoria Categoria { get; set;}
        public List<Comentario> Comentario { get; set;}
        public List<Etiqueta> Etiqueta { get; set;}
    }
}
