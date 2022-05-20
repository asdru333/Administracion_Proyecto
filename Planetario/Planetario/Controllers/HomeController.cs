using System.Web.Mvc;

namespace Planetario.Controllers
{
    public class HomeController : Controller
    {
      
        public ActionResult InformacionBasica()
        {
            ViewBag.Message = "La informacion basica del planetario.";
            return View("InformacionBasica","_LayoutAlternativo");
        }

        public ActionResult HorariosParqueoTransporte()
        {
            ViewBag.Message = "Informacion sobre horarios, parqueo  transporte";

            return View();
        }

        public ActionResult PanelDeAdministracion()
        {
            return View();
        }

        public ActionResult CompararCuerpos()
        {
            return View("CompararCuerpos", "_LayoutAlternativo");
        }
    }
}