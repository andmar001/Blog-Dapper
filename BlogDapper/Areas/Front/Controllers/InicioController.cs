using BlogDapper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogDapper.Areas.Front.Controllers
{
    //configuración por areas
    [Area("Front")]
    public class InicioController : Controller
    {
        private readonly ILogger<InicioController> _logger;

        public InicioController(ILogger<InicioController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}