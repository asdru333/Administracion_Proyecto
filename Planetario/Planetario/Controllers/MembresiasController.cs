using System.Web.Mvc;
using Planetario.Handlers;
using Planetario.Interfaces;
using Planetario.Models;

namespace Planetario.Controllers
{
    public class MembresiasController : Controller
    {
        readonly MembresiasInterfaz membresiasInterfaz;
        readonly CookiesInterfaz cookiesInterfaz;

        public MembresiasController()
        {
            membresiasInterfaz = new MembresiasHandler();
            cookiesInterfaz = new CookiesHandler();
        }

        public MembresiasController(MembresiasInterfaz ventas, CookiesInterfaz cookies)
        {
            membresiasInterfaz = ventas;
            cookiesInterfaz = cookies;
        }

        public ActionResult Comprar()
        {          
            PersonaHandler personasHandler = new PersonaHandler();
            string correoUsuario = cookiesInterfaz.CorreoUsuario();
            string membresia = personasHandler.ObtenerMembresia(correoUsuario);
            ViewBag.Membresia = membresia;
            return View();
        }
        
        [HttpGet]
        public ActionResult Pago(string membresia)
        {
            ViewBag.Membresia = membresia;

            if(membresia == "Lunar")
            {
                ViewBag.Precio = 5000;
                ViewBag.IVA = 650;
                ViewBag.PrecioTotal = 5650;
            }
            else if (membresia == "Solar")
            {
                ViewBag.Precio = 10000;
                ViewBag.IVA = 1300;
                ViewBag.PrecioTotal = 11300;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Pago(InscripcionModel inscripcion)
        {
            string membresia = Request.Form["membresia"];
            string mensaje = "Hubo error";
            ViewBag.Membresia = membresia;

            if (membresia == "Lunar")
            {
                ViewBag.Precio = 5000;
                ViewBag.IVA = 650;
                ViewBag.PrecioTotal = 5650;
            }
            else if (membresia == "Solar")
            {
                ViewBag.Precio = 10000;
                ViewBag.IVA = 1300;
                ViewBag.PrecioTotal = 11300;
            }
            string correo = cookiesInterfaz.CorreoUsuario();
            ViewBag.ExitoAlActualizar = false;
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.ExitoAlCrear = membresiasInterfaz.ActualizarMembresia(correo, membresia);
                    if (ViewBag.ExitoAlCrear)
                    {
                        mensaje = "Su membresía ahora es: " + membresia + "!";
                    }
                    else
                    {
                        ViewBag.Mensaje = "Hubo un error en el servidor";
                    }
                }
                else
                {
                    ViewBag.Mensaje = "Insertó sus datos incorrectamente";
                    return View();
                }
            }
            catch
            {
                ViewBag.Mensaje = "Hubo un error en el servidor";
            }
            return this.Satisfactorio(mensaje);
        }

        public ActionResult Satisfactorio(string mensaje)
        {
            ViewBag.Mensaje = mensaje;
            return View("Satisfactorio");
        }
    }
}