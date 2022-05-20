using Planetario.Handlers;
using System.Web.Mvc;

namespace Planetario.Controllers
{
    public class DatosController : Controller
    {
        public ActionResult Topicos(string categoria)
        {
            DatosHandler accesoDatos = new DatosHandler();
            JsonResult jsonTopicos;
            if(categoria == null)
            {
                jsonTopicos = Json(accesoDatos.ObtenerTodosLosTopicos(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                jsonTopicos = Json(accesoDatos.ObtenerTopicosPorCategoria(categoria), JsonRequestBehavior.AllowGet);
            }
            return jsonTopicos;
        }

        [HttpGet]
        public ActionResult Categorias()
        {
            DatosHandler accesoDatos = new DatosHandler();
            return Json(accesoDatos.categorias, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DiasDeLaSemana()
        {
            DatosHandler accesoDatos = new DatosHandler();
            return Json(accesoDatos.diasDeLaSemana, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Paises()
        {
            DatosHandler accesoDatos = new DatosHandler();
            return Json(accesoDatos.paises, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult RespuestasEvaluacion()
        {
            DatosHandler accesoDatos = new DatosHandler();
            return Json(accesoDatos.evaluacion, JsonRequestBehavior.AllowGet);
        }
    }
}