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
    }
}
