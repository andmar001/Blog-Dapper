using BlogDapper.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace BlogDapper.Repositorio
{
    public class ComentarioRepositorio : IComentarioRepositorio
    {
        private readonly IDbConnection _bd;
        //conexión a base de datos
        public ComentarioRepositorio(IConfiguration configuration)
        {
            _bd = new SqlConnection(configuration.GetConnectionString("ConexionSQLLocalDB"));
        }
        public List<Comentario> GetComentarios()
        {
            var sql = "SELECT * FROM Comentario";

            return _bd.Query<Comentario>(sql).ToList();
        }
        public Comentario GetComentario(int id)
        {
            var sql = "SELECT * FROM Comentario WHERE IdComentario = @IdComentario";
            return _bd.Query<Comentario>(sql, new { IdComentario = id }).Single();

        }
        //El comentario lo crea el usuario
        public Comentario CrearComentario(Comentario comentario)
        {
            var sql = "INSERT INTO Comentario(Titulo, Mensaje,ArticuloId, FechaCreacion) " +
                      "VALUES (@Titulo, @Mensaje,@ArticuloId, @FechaCreacion)";
            _bd.Execute(sql, new
            {
                comentario.Titulo,
                comentario.Mensaje,
                comentario.ArticuloId,
                FechaCreacion = DateTime.Now,
            });

            return comentario;
        }
        public Comentario ActualizarComentario(Comentario comentario)
        {
            var sql = "UPDATE Comentario SET Titulo=@Titulo, Mensaje=@Mensaje  WHERE IdComentario = @IdComentario";
            _bd.Execute(sql, comentario);

            return comentario;
        }
        public void BorrarComentario(int id)
        {
            var sql = "DELETE FROM Comentario WHERE IdComentario = @IdComentario";
            _bd.Execute(sql, new { IdComentario = id });
        }
    }
}
