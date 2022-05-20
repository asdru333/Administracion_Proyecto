using System.Web.Mvc;
using Planetario.Handlers;
using Planetario.Models;

namespace Planetario.Controllers
{
    public class ClientesController : Controller
    {

        [HttpGet]
        public ActionResult Registro()
        {
            DatosHandler dataHandler = new DatosHandler();
            ViewBag.paises = dataHandler.SelectListPaises();
            ViewBag.generos = dataHandler.SelectListGeneros();
            return View();
        }

        [HttpPost]
        public ActionResult Registro(PersonaModel persona)
        {
            ViewBag.ExitoAlCrear = false;
            try
            {
                if (ModelState.IsValid)
                {
                    ClientesHandler accesoDatos = new ClientesHandler();

                    ViewBag.ExitoAlCrear = accesoDatos.InsertarCliente(persona);
                    if (ViewBag.ExitoAlCrear)
                    {
                        ViewBag.Message = persona.nombre + " tu registro fue exitoso.";
                        ModelState.Clear();
                    }
                }
                return RedirectToAction("IniciarSesion", "Personas");
            }
            catch
            {
                ViewBag.Message = "No pudimos completar tu registro.";
                return View();
            }
        }
    }
}