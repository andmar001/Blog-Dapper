using BlogDapper.Models;
using BlogDapper.Repositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;

namespace BlogDapper.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly ISliderRepositorio _repoSlider; 
        private readonly IWebHostEnvironment _hostingEnvironment;
        public SlidersController(ISliderRepositorio repoSlider, IWebHostEnvironment hostingEnvironment)
        {
            _repoSlider = repoSlider;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
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
        public IActionResult Crear([Bind("IdSlider, Nombre, UrlImagen")] Slider slider)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                if (slider.IdSlider == 0)
                {
                    // nuevo slider
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\slider");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    slider.UrlImagen = @"\imagenes\slider\" + nombreArchivo + extension;
                    _repoSlider.CrearSlider(slider);
                    return RedirectToAction(nameof(Index));

                }
                //Esta línea valida el modelo si es "false" retorna a la vista crear pero del GET, o sea al formulario               
                return RedirectToAction(nameof(Crear));
            }
            return View(slider);
        }

        [HttpGet]
        public IActionResult Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = _repoSlider.GetSlider(id.GetValueOrDefault());
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        [HttpPost]
        public IActionResult Editar(int id, [Bind("IdSlider, Nombre, UrlImagen")] Slider slider)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                var articuloDesdeDb = _repoSlider.GetSlider(id);

                if (archivos.Count() > 0)
                {
                    //Editamos o cambiamos la imagen del artículo
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\slider");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                    var rutaImagen = Path.Combine(rutaPrincipal, articuloDesdeDb.UrlImagen.TrimStart('\\'));

                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    //Subimos el nuevo archivo
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + nuevaExtension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    slider.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + nuevaExtension;

                    _repoSlider.ActualizarSlider(slider);
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    //Aquí es cuando la imagen ya existe y no se reemplaza
                    slider.Nombre = articuloDesdeDb.Nombre;
                }

                _repoSlider.ActualizarSlider(slider);
                return RedirectToAction(nameof(Index));

            }
            return RedirectToAction(nameof(Editar));

        }

        #region Javascript
        [HttpGet]
        public IActionResult GetSliders()
        {
            //return Json(new { data = _repoArticulo.GetArticulos()});
            return Json(new { data = _repoSlider.GetSliders() });
        }

        [HttpDelete]
        public IActionResult BorrarSlider(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                _repoSlider.BorrarSlider(id.GetValueOrDefault());
                return Json(new { success = true, message = "Slider borrado correctamente" });
            }
        }

        #endregion
    }
}
