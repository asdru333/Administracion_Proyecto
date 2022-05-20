using System.Web.Mvc;
using Planetario.Interfaces;
using Planetario.Handlers;

namespace Planetario.Controllers
{
    public class ReportesController : Controller
    {
        readonly ReportesInterfaz AccesoDatos;
        readonly DatosInterfaz AccesoDatosApp;

        //Constructores
        public ReportesController()
        {
            AccesoDatos = new ReportesHandler();
            AccesoDatosApp = new DatosHandler();
        }

        public ReportesController(ReportesInterfaz servicio)
        {
            AccesoDatos = servicio;
            AccesoDatosApp = new DatosHandler();
        }

        public ReportesController(ReportesInterfaz servicio, DatosInterfaz servicioDatos)
        {
            AccesoDatos = servicio;
            AccesoDatosApp = servicioDatos;
        }

        //Vistas
        [HttpGet]
        public ActionResult Reporte()
        {
            ViewBag.listaDeCategorias = AccesoDatos.ObtenerTodasLasCategorias();
            ViewBag.listaDeProductos = AccesoDatos.ObtenerTodosLosProductos();
            return View("Reporte");
        }

        [HttpPost]
        public ActionResult Reporte(string nombre, string fechaInicio, string fechaFinal)
        {
            ViewBag.listaFechas = AccesoDatos.ObtenerTodosLosProductosFiltradosPorCategoriaFechasVentas(nombre, fechaInicio, fechaFinal);
            ViewBag.listaVentas = AccesoDatos.ObtenerTodosLosProductosFiltradosPorCategoriaCantidadVentas(nombre, fechaInicio, fechaFinal);
            ViewBag.listaDeCategorias = AccesoDatos.ObtenerTodasLasCategorias();
            ViewBag.listaDeProductos = AccesoDatos.ObtenerTodosLosProductos();
            return View("Reporte");
        }


        public ActionResult ReporteMercadeo()
        {
            ViewBag.listaDeCategorias = AccesoDatos.ObtenerTodasLasCategorias();
            ViewBag.listaGeneros = AccesoDatosApp.SelectListGeneros();
            ViewBag.listaPublicos = AccesoDatosApp.SelectListPublicos();
            return View("ReporteMercadeo");
        }

        [HttpGet]
        public JsonResult ObtenerFiltroPorRanking(string orden, string fechaInicial, string fechaFinal)
        {
            var resultadoJson = AccesoDatos.ObtenerTodosLosProductosFiltradosPorRanking(fechaInicial, fechaFinal, orden);
            return Json(resultadoJson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerDatosExtranjeros(string categoria)
        {
            var resultadoJson = AccesoDatos.ConsultaPorCategoriasPersonaExtranjeras(categoria);
            return Json(resultadoJson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerDatosPorGeneroYEdad(string categoria, string publico, string genero)
        {
            var resultadoJson = AccesoDatos.ConsultaPorCategoriaProductoGeneroEdad(categoria, genero, publico);
            return Json(resultadoJson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerParesProductos(string publico, string membresia)
        {
            var resultadoJson = AccesoDatos.ConsultaProductosCompradosJuntos(publico, membresia);
            return Json(resultadoJson, JsonRequestBehavior.AllowGet);
        }
    }
}