using BlogDapper.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogDapper.Repositorio
{
    public interface ICategoriaRepositorio
    {
        Categoria GetCategoria(int id);
        List<Categoria> GetCategorias();
        Categoria CrearCategoria(Categoria categoria);
        Categoria ActualizarCategoria(Categoria categoria);
        void BorrarCategoria(int id);
        //método especial para el dropdown con la lista de categorias en la vista articulos,
        //se debe crear aqui para invocarse desde el controlador articulos
        IEnumerable<SelectListItem> GetListaCategorias();
    }
}
