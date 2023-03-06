using BlogDapper.Models;
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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear([Bind("IdCategoria,Nombre,FechaCreacion")] Categoria categoria)
        {
            //validaciones del modelo
            if(ModelState.IsValid)
            {
                _repoCategoria.CrearCategoria(categoria);
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }
        
        //Este método viene del categorias.js - solo recupera la data por get
        [HttpGet]
        public IActionResult Editar(int? id)
        {
            //validaciones
            if(id == null)
            {
                return NotFound();
            }
            
            var categoria = _repoCategoria.GetCategoria(id.GetValueOrDefault());
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, [Bind("IdCategoria,Nombre,FechaCreacion")] Categoria categoria)
        {
            if (id != categoria.IdCategoria)
            {
                return NotFound();
            }
            //validaciones del modelo
            if (ModelState.IsValid)
            {
                _repoCategoria.ActualizarCategoria(categoria);
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
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
