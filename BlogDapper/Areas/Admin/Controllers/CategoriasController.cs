using BlogDapper.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace BlogDapper.Areas.Admin.Controllers
{
    [Area("Admin")] //pertenece al area de admin
    public class CategoriasController : Controller
    {
        private readonly ICategoriaRepositorio _repoCategoria;
        public CategoriasController(ICategoriaRepositorio repoCategoria)
        {
            _repoCategoria = repoCategoria;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        #region
        [HttpGet]
        public IActionResult GetCategorias() 
        {
            return Json(new { data = _repoCategoria.GetCategorias() });
        }
        #endregion
    }
}
