using BlogDapper.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogDapper.Repositorio
{
    public interface IEtiquetaRepositorio
    {
        Etiqueta GetEtiqueta(int id);
        List<Etiqueta> GetEtiquetas();
        Etiqueta CrearEtiqueta(Etiqueta etiqueta);
        Etiqueta ActualizarEtiqueta(Etiqueta etiqueta);
        void BorrarEtiqueta(int id);
        //método especial para el dropdown con la lista de Etiquetas en articulo
        IEnumerable<SelectListItem> GetListaEtiquetas();

        //método especial para la accion de asignar etiquetas
        
        //método especial para obtener los articulos con las etiquetas asignadas
        List<Articulo> GetArticuloEtiquetas();
        
    }
}
