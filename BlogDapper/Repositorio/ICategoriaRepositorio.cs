using BlogDapper.Models;

namespace BlogDapper.Repositorio
{
    public interface ICategoriaRepositorio
    {
        Categoria GetCategoria(int id);
        List<Categoria> GetCategorias();
        Categoria CrearCategoria(Categoria categoria);
        Categoria ActualizarCategoria(Categoria categoria);
        void BorrarCategoria(int id);
    }
}
