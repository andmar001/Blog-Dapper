using BlogDapper.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Net.WebSockets;

namespace BlogDapper.Repositorio
{
    public class SliderRepositorio : ISliderRepositorio
    {
        private readonly IDbConnection _bd;
        public SliderRepositorio(IConfiguration configuration)
        {
            _bd = new SqlConnection(configuration.GetConnectionString("ConexionSQLLocalDB"));
        }
        public Slider GetSlider(int id)
        {
            var sql = "SELECT * FROM Slider WHERE IdSlider=@IdSlider";

            var slider = _bd.Query<Slider>(sql, new
            {
                IdSlider = id,
            }).Single();
            
            return slider;
        }

        public List<Slider> GetSliders()
        {
            var sql = "SELECT * FROM Slider";
            var sliders = _bd.Query<Slider>(sql).ToList();

            return sliders;
        }
        public Slider CrearSlider(Slider slider)
        {
            var sql = "INSERT INTO Slider (Nombre,UrlImagen) VALUES (@Nombre,@UrlImagen)";

            _bd.Execute(sql, new
            {
                slider.Nombre,
                slider.UrlImagen
            });

            return slider;
        }
        public Slider ActualizarSlider(Slider slider)
        {
            var sql = "UPDATE Slider SET (Nombre=@Nombre,UrlImagen=@UrlImagen) WHERE IdSlider=@IdSlider";

            _bd.Execute(sql, slider);
            
            return slider;
        }

        public void BorrarSlider(int id)
        {
            var sql = "DELETE FROM Slider WHERE IdSlider =@IdSlider";
            _bd.Execute(sql, new
            {
                IdSlider = id
            });
        }
    }
}
