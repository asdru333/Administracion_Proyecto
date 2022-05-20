using System.Web.Mvc;
using Planetario.Handlers;
using Planetario.Models;

namespace Planetario.Controllers
{
    public class CuestionarioController : Controller
    {
        public ActionResult ListaCuestionarios()
        {
            CuestionarioHandler AcessoDatos = new CuestionarioHandler();
            ViewBag.ListaCuestionarios = AcessoDatos.obtenerCuestinariosSimple();
            ViewBag.Dificultad = "";
            return View();
        }

        [HttpPost]
        public ActionResult ListaCuestionarios(string dificultad)
        {
            CuestionarioHandler AcessoDatos = new CuestionarioHandler();
            ViewBag.ListaCuestionarios = AcessoDatos.obtenerCuestinarioPorDificultad(dificultad);
            ViewBag.Dificultad = dificultad;
            return View();
        }

        public ActionResult VerCuestionario(string nombre)
        {
            CuestionarioHandler AcessoDatos = new CuestionarioHandler();
            ViewBag.Cuestionario = AcessoDatos.buscarCuestionario(nombre);
            return View();
        }

        public ActionResult agregarCuestionario()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult agregarCuestionario(CuestionarioModel cuestionario)
        {
            ViewBag.ExitoAlCrear = false;
            try
            {
                if (ModelState.IsValid)
                {
                    CuestionarioHandler accesoDatos = new CuestionarioHandler();
                    cuestionario.EmbedHTML = modificarEmbed(cuestionario.EmbedHTML);
                    ViewBag.ExitoAlCrear = accesoDatos.agregarCuestionario(cuestionario);
                    if (ViewBag.ExitoAlCrear)
                    {
                        ViewBag.Message = "El cuestionario " + cuestionario.NombreCuestionario + " fue creada con éxito.";
                        ModelState.Clear();
                    }
                }
                return View();
            }
            catch
            {
                ViewBag.Message = "Hubo un error al crear el cuestionario " + cuestionario.NombreCuestionario;
                return View();
            }
        }

        public string modificarEmbed(string embed)
        {
            int inicio = embed.IndexOf("https");
            embed = embed.Substring(inicio, embed.IndexOf("true") - inicio + 4);
            return embed;
        }
    }
}