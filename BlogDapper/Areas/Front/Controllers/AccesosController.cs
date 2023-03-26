using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace BlogDapper.Areas.Front.Controllers
{
    [Area("Front")]
    public class AccesosController : Controller
    {
        private readonly IDbConnection _bd;
        public AccesosController(IConfiguration configuration)
        {
            _bd = new SqlConnection(configuration.GetConnectionString("ConexionSQLLocalDB"));
        }
        [HttpGet]
        public IActionResult Acceso()
        {
            return View();
        }
    }
}
