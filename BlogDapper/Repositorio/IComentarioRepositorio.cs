using BlogDapper.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogDapper.Repositorio
{
    public interface IComentarioRepositorio
    {
        Comentario GetComentario(int id);
        List<Comentario> GetComentarios();
        Comentario CrearComentario(Comentario comentario);
        Comentario ActualizarComentario(Comentario comentario);
        void BorrarComentario(int id);
        //Se agrega para hacer la relacion entre las tablas
        List<Comentario> GetComentarioArticulo();
    }
}
