using System.Web.Mvc;
using Planetario.Handlers;
using Planetario.Models;

namespace Planetario.Controllers
{
    public class PreguntasFrecuentesController : Controller {
        public ActionResult PreguntasFrecuentes() {
            PreguntasFrecuentesHandler accesoDatos = new PreguntasFrecuentesHandler();
            ViewBag.preguntasFrecuentes = accesoDatos.ObtenerPreguntasFrecuentes();
            ViewBag.categorias = accesoDatos.ObtenerCategorias();
            return View();
        }

        [HttpGet]
        public ActionResult AgregarNuevaPregunta() {                   
            return View();
        }

        [HttpPost]
        public ActionResult AgregarNuevaPregunta(PreguntasFrecuentesModel nuevaPregunta) {
            ViewBag.ExitoAlCrear = false;
            try
            {
                if (ModelState.IsValid) {
                    PreguntasFrecuentesHandler accesoDatos = new PreguntasFrecuentesHandler();
                    ViewBag.ExitoAlCrear = accesoDatos.agregarNuevaPregunta(nuevaPregunta);

                    if(ViewBag.ExitoAlCrear) {
                        ViewBag.Message = "La pregunta fue añadida satisfactoriamente!";
                        ModelState.Clear();
                    }
                }
                else
                {
                    ViewBag.Message = "El modelo no es valido!";
                }
                return View();
            }
            catch
            {
                ViewBag.Message = "Sucedio algo inesperado y no fue posible añadir la pregunta.";
                return View();
            }
        }


    } 
}