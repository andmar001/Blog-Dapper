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
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ArticulosController(ICategoriaRepositorio repoCategoria, IArticuloRepositorio repoArticulo, IWebHostEnvironment hostingEnvironment)
        {
            _repoCategoria = repoCategoria;
            _repoArticulo = repoArticulo;
            _hostingEnvironment = hostingEnvironment;  //to load files
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
        public IActionResult Crear([Bind("IdArticulo,Titulo,Descripcion,Imagen,Estado,CategoriaId,FechaCreacion")] Articulo articulo)
        {
            //validaciones del modelo
            if(ModelState.IsValid)
            {
                //subida de archivos
                string rutaPrincipal = _hostingEnvironment.WebRootPath; //www
                var archivos = HttpContext.Request.Form.Files;

                if(articulo.IdArticulo == 0)
                {
                    //creamos un nuevo articulo
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    using ( var filesStream = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(filesStream);
                    }
                    //guardar ruta de la imagen
                    articulo.Imagen = @"\imagenes\articulos\" + nombreArchivo + extension;
                    _repoArticulo.CrearArticulo(articulo);

                    return RedirectToAction(nameof(Index));

                }
                //esta linea valida el modelo si es "false" retorna a la vista pero del GET, o sea el formulario
                return RedirectToAction(nameof(Crear));
            }
            return View(articulo);
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
