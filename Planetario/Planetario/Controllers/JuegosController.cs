using Planetario.Handlers;
using Planetario.Interfaces;
using System.Web.Mvc;

namespace Planetario.Controllers
{
    public class JuegosController : Controller
    {
        readonly CookiesInterfaz cookiesInterfaz;

        public JuegosController()
        {
            cookiesInterfaz = new CookiesHandler();
        }

        public ActionResult Lista()
        {
            PersonaHandler personasHandler = new PersonaHandler();
            string correoUsuario = cookiesInterfaz.CorreoUsuario();
            string membresia = personasHandler.ObtenerMembresia(correoUsuario);
            ViewBag.Membresia = membresia;
            return View();
        }

        public ActionResult UFO2048()
        {
            return View();
        }

        public ActionResult ETBrain()
        {
            return View();
        }

        public ActionResult ConectaPlanetas()
        {
            return View();
        }

        public ActionResult RapidMath()
        {
            return View();
        }

        public ActionResult SistemaInteractivo()
        {
            return View();
        }

        public ActionResult VideoQuiz()
        {
            return View();
        }
    }
}