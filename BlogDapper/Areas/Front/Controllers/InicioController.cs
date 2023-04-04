using BlogDapper.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BlogDapper.Areas.Front.Controllers
{
    //configuración por areas
    [Area("Front")]
    public class InicioController : Controller
    {
        private readonly ILogger<InicioController> _logger;
        private readonly IDbConnection _bd;

        public InicioController(IConfiguration configuration, ILogger<InicioController> logger)
        {
            _bd = new SqlConnection(configuration.GetConnectionString("ConexionSQLLocalDB"));
            _logger = logger;
        }

        public IActionResult Index()
        {
            var sqlSlider = @"SELECT * FROM Slider ORDER BY IdSlider DESC";
            ViewData["ListaSlider"] = _bd.Query<Slider>(sqlSlider).ToList();

            var sqlArticulos = @"SELECT * FROM Articulo WHERE Estado=@Estado ORDER BY IdArticulo DESC";
            var articulos = _bd.Query<Articulo>(sqlArticulos, new
            {
                //solo articulos validos 
                @Estado = 1
            }).ToList();

            var sqlCategorias = @"SELECT * FROM Categoria ORDER BY IdCategoria DESC";
            ViewData["ListaCategorias"] = _bd.Query<Categoria>(sqlCategorias).ToList();

            //con esta validación sabemos si estamos en el home o no
            ViewBag.IsHome = true;

            return View(articulos);

        }

        [HttpGet]
        public IActionResult Detalle(int id)
        {
            var sql = @"SELECT * FROM Articulo WHERE IdArticulo=@IdArticulo";
            var articulo = _bd.Query<Articulo>(sql, new
            {
                IdArticulo = id
            }).Single();

            return View(articulo); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}