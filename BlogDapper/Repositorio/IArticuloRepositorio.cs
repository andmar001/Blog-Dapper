using BlogDapper.Models;

namespace BlogDapper.Repositorio
{
    public interface IArticuloRepositorio
    {
        Articulo GetArticulo(int id);
        List<Articulo> GetArticulos();
        Articulo CrearArticulo(Articulo articulo);
        Articulo ActualizarArticulo(Articulo articulo);
        void BorrarArticulo(int id);
        //se agrega nuevo método para obtener la relación entre articulo y categoría
        List<Articulo> GetArticuloCategoria();
    }
}
