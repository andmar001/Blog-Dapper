using BlogDapper.Models;
using BlogDapper.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace BlogDapper.Areas.Admin.Controllers
{
    [Area("Admin")] //pertenece al area de admin
    public class ArticulosController : Controller
    {
        private readonly ICategoriaRepositorio _repoCategoria;
        private readonly IArticuloRepositorio _repoArticulo;
        public ArticulosController(ICategoriaRepositorio repoCategoria, IArticuloRepositorio repoArticulo)
        {
            _repoCategoria = repoCategoria;
            _repoArticulo = repoArticulo;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Crear()
        {
            ViewBag.SelectList = _repoCategoria.GetListaCategorias();
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

        #region - interactua con js
        [HttpGet]
        public IActionResult GetArticulos() 
        {
            return Json(new { data = _repoArticulo.GetArticulos() });
        }
        
        [HttpDelete]
        public IActionResult BorrarArticulo(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                _repoArticulo.BorrarArticulo(id.GetValueOrDefault());
                return Json(new { success = true, message ="Articulo borrado correctamente" });
            }
        }
        #endregion
    }
}
