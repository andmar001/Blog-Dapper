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
        private readonly IEtiquetaRepositorio _repoEtiqueta;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ArticulosController(ICategoriaRepositorio repoCategoria, IArticuloRepositorio repoArticulo, IEtiquetaRepositorio repoEtiqueta, IWebHostEnvironment hostingEnvironment)
        {
            _repoCategoria = repoCategoria;
            _repoArticulo = repoArticulo;
            _repoEtiqueta = repoEtiqueta;
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
            
            var articulo = _repoArticulo.GetArticulo(id.GetValueOrDefault());
            if (articulo == null)
            {
                return NotFound();
            }

            ViewBag.SelectList = _repoCategoria.GetListaCategorias();
            return View(articulo);
        }

        [HttpPost]
        public IActionResult Editar(int id, [Bind("IdArticulo,Titulo,Descripcion,Imagen,Estado,CategoriaId,FechaCreacion")] Articulo articulo)
        {
            //validaciones del modelo
            if (ModelState.IsValid)
            {
                //subida de archivos
                string rutaPrincipal = _hostingEnvironment.WebRootPath; //www
                var archivos = HttpContext.Request.Form.Files;

                //obtener articulo de la base de datos
                var articuloDesdeDB = _repoArticulo.GetArticulo(id);

                if (archivos.Count() > 0)
                {
                    //Editamos o cambiamos la imagen del articulo
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                    var rutaImagen = Path.Combine(rutaPrincipal, articuloDesdeDB.Imagen.TrimStart('\\'));

                    //Eliminar en caso q exista
                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    //subimos el nuevo archivo
                    using (var filesStream = new FileStream(Path.Combine(subidas, nombreArchivo + nuevaExtension), FileMode.Create))
                    {
                        archivos[0].CopyTo(filesStream);
                    }
                    //guardar ruta de la imagen
                    articulo.Imagen = @"\imagenes\articulos\" + nombreArchivo + nuevaExtension;

                    _repoArticulo.ActualizarArticulo(articulo);

                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    //aqui es cuando la imagen ya existe y no se reemplaza(cuando no se cambio la imagen en la edición)
                    articulo.Imagen = articuloDesdeDB.Imagen;
                }

                _repoArticulo.ActualizarArticulo(articulo);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Editar));
        }

        //Para la parte de asignar etiquetas a un articulo 
        [HttpGet]
        public IActionResult AsignarEtiquetas(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var articulo = _repoArticulo.GetArticulo(id.GetValueOrDefault());

            if (articulo == null)
            {
                return NotFound();
            }
            //si, si existe - dropdown
            ViewBag.SelectList = _repoEtiqueta.GetListaEtiquetas();

            return View(articulo);
        }

        #region - interactua con js
        [HttpGet]
        public IActionResult GetArticulos() 
        {
            //return Json(new { data = _repoArticulo.GetArticulos() });
            return Json(new { data = _repoArticulo.GetArticuloCategoria() });
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
