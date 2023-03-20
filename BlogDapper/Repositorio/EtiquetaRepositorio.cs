using BlogDapper.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;

namespace BlogDapper.Repositorio
{
    public class EtiquetaRepositorio : IEtiquetaRepositorio
    {
        private readonly IDbConnection _bd;
        //conexión a base de datos
        public EtiquetaRepositorio(IConfiguration configuration)
        {
            _bd = new SqlConnection(configuration.GetConnectionString("ConexionSQLLocalDB"));
        }
        public List<Etiqueta> GetEtiquetas()
        {
            var sql = "SELECT * FROM Etiqueta";

            return _bd.Query<Etiqueta>(sql).ToList();
        }
        public Etiqueta GetEtiqueta(int id)
        {
            var sql = "SELECT * FROM Etiqueta WHERE IdEtiqueta = @IdEtiqueta";
            return _bd.Query<Etiqueta>(sql, new { IdEtiqueta = id }).Single();

        }
        public Etiqueta CrearEtiqueta(Etiqueta etiqueta)
        {
            var sql = "INSERT INTO Etiqueta(NombreEtiqueta,FechaCreacion) VALUES (@NombreEtiqueta,@FechaCreacion)";
            _bd.Execute(sql, new
            {
                etiqueta.NombreEtiqueta,
                FechaCreacion = DateTime.Now,
            });

            return etiqueta;
        }
        public Etiqueta ActualizarEtiqueta(Etiqueta etiqueta)
        {
            var sql = "UPDATE Etiqueta SET NombreEtiqueta=@NombreEtiqueta WHERE IdEtiqueta = @IdEtiqueta";
            _bd.Execute(sql, etiqueta);

            return etiqueta;
        }
        public void BorrarEtiqueta(int id)
        {
            var sql = "DELETE FROM Etiqueta WHERE IdEtiqueta = @IdEtiqueta";
            _bd.Execute(sql, new { IdEtiqueta = id });
        }

        public IEnumerable<SelectListItem> GetListaEtiquetas()
        {
            var sql = "SELECT * FROM Etiqueta";
            var lista = _bd.Query<Etiqueta>(sql).ToList();
            SelectList listaEtiquetas = new SelectList(lista, "IdEtiqueta", "NombreEtiqueta");
            return listaEtiquetas;
        }

        public List<Articulo> GetArticuloEtiquetas()
        {
            throw new NotImplementedException();
        }

        //etiquetas asignadas a un articulo
        //public List<Articulo> GetArticuloEtiquetas()
        //{
        //    var sql = "SELECT"

        //}
    }
}
