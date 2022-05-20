using System.Web.Mvc;
using Planetario.Handlers;
using Planetario.Models;

namespace Planetario.Controllers
{
    public class EventosController : Controller
    {
        public ActionResult AgregarEvento()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarEvento(EventoModel evento)
        {
            ViewBag.ExitoAlCrear = false;
            try
            {
                if (ModelState.IsValid)
                {
                    EventosHandler accesoDatos = new EventosHandler();
                    ViewBag.ExitoAlCrear = accesoDatos.InsertarEvento(evento);
                    if (ViewBag.ExitoAlCrear)
                    {
                        ViewBag.mensaje = "El evento " + evento.Titulo + " fue creado con éxito.";
                        ModelState.Clear();
                    }
                }
                return View();
            }
            catch
            {
                ViewBag.Message = "Hubo un error al crear el cuestionario " + evento.Titulo;
                return View();
            }
        }

        public ActionResult VerEvento(string titulo)
        {
            EventosHandler accesoDatos = new EventosHandler();
            ViewBag.Evento = accesoDatos.ObtenerUnEvento(titulo);
            return View(ViewBag.Evento);
        }

    }
}