using Planetario.Handlers;
using Planetario.Models;
using Planetario.Interfaces;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Planetario.Controllers
{
    public class EvaluacionController : Controller
    {
        readonly EvaluacionInterfaz accesoDatos;

        public EvaluacionController( )
        {
            accesoDatos = new EvaluacionHandler();
        }

        public EvaluacionController(EvaluacionInterfaz _servicio)
        {
            accesoDatos = _servicio;
        }

        public ActionResult CuestionarioEvaluacion()
        {
            ViewBag.Cuestionario = accesoDatos.ObtenerCuestionarioRecibir("Califica tu experiencia");
            return View();
        }

        [HttpPost]
        public ActionResult CuestionarioEvaluacion(CuestionarioEvaluacionRecibirModel cuestionario)
        {
            ViewBag.ExitoAlCrear = false;
            ViewBag.Cuestionario = accesoDatos.ObtenerCuestionarioRecibir("Califica tu experiencia");
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.ExitoAlCrear = accesoDatos.InsertarRespuestas(cuestionario);
                    if(cuestionario.Comentario[0] != "")
                        ViewBag.ExitoAlCrear = accesoDatos.InsertarComentario(cuestionario);
                    ViewBag.ExitoAlCrear = accesoDatos.InsertarFuncionalidadesEvaluadas(cuestionario);
                    if (ViewBag.ExitoAlCrear)
                    {
                        ViewBag.Message = "Se respondió el cuestionario con exito.";                   
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Message = "Usted ya ha respondido éste cuestionario.";
                    }
                }
                else
                {
                    ViewBag.Message = "El cuestionario tiene errores. Por favor revise sus respuestas.";
                }
                return View();
            }
            catch
            {
                ViewBag.Message = "Hubo un error al responder el cuestionario.";
                return View();
            }
        }

        private void ObtenerResumenesEvaluaciones(string fechaIncio, string fechaFin)
        {
            List<string> preguntasNavegacion = new List<string>
            {
                "El sitio web es estéticamente agradable",
                "Navegar por las distintas páginas del sitio web es fácil"
            };

            List<string> opcionesNavegacion = new List<string>
            {
                "En desacuerdo",
                "En desacuerdo"
            };

            List<string> preguntasCompras = new List<string>();
            preguntasCompras.Add("Comprar en el sitio web es fácil");
            preguntasCompras.Add("Los precios de los productos y actividades son apropiados");

            List<string> opcionesComprasCompras = new List<string>();
            opcionesComprasCompras.Add("Muy de acuerdo");
            opcionesComprasCompras.Add("Muy en desacuerdo");

            ViewBag.Navegacion = accesoDatos.ObtenerRelaciones("Califica tu experiencia", preguntasNavegacion, opcionesNavegacion, fechaIncio, fechaFin);
            ViewBag.Compras = accesoDatos.ObtenerRelaciones("Califica tu experiencia", preguntasCompras, opcionesComprasCompras, fechaIncio, fechaFin);
        }

        [HttpGet]
        public ActionResult MostrarCuestionarioEvaluacion()
        {
            string nombreCuestionario = "Califica tu experiencia";
            ViewBag.Cuestionario = accesoDatos.ObtenerCuestionarioMostrar(nombreCuestionario);
            ViewBag.Cantidad = accesoDatos.ObtenerCantidadPersonas(nombreCuestionario);

            DatosController datos = new DatosController();
            ViewBag.Opciones = datos.RespuestasEvaluacion();

            ViewBag.RespuestasEsteticamente = accesoDatos.ObtenerCantidadRespuestasPorPregunta(1);
            ViewBag.RespuestasNavegar = accesoDatos.ObtenerCantidadRespuestasPorPregunta(2);
            ViewBag.RespuestasComprar = accesoDatos.ObtenerCantidadRespuestasPorPregunta(3);
            ViewBag.RespuestasPrecios = accesoDatos.ObtenerCantidadRespuestasPorPregunta(4);
            ViewBag.RespuestasSatisfecho = accesoDatos.ObtenerCantidadRespuestasPorPregunta(5);

            ViewBag.Comentarios = accesoDatos.ObtenerComentariosDeCuestionario("Califica tu experiencia");

            ObtenerResumenesEvaluaciones("2021-01-01", "2021-12-31");

            ViewBag.Fechas = "En general se obtienen los siguientes resultados:";

            return View();
        }

        [HttpPost]
        public ActionResult MostrarCuestionarioEvaluacion(string fechaInicio, string fechaFin)
        {
            ViewBag.Cuestionario = accesoDatos.ObtenerCuestionarioMostrar("Califica tu experiencia");
            ViewBag.Cantidad = accesoDatos.ObtenerCantidadPersonasPorFecha("Califica tu experiencia", fechaInicio, fechaFin);

            DatosController datos = new DatosController();
            ViewBag.Opciones = datos.RespuestasEvaluacion();

            ViewBag.RespuestasEsteticamente = accesoDatos.ObtenerCantidadRespuestasPorPreguntaYFecha(1, fechaInicio, fechaFin);
            ViewBag.RespuestasNavegar = accesoDatos.ObtenerCantidadRespuestasPorPreguntaYFecha(2, fechaInicio, fechaFin);
            ViewBag.RespuestasComprar = accesoDatos.ObtenerCantidadRespuestasPorPreguntaYFecha(3, fechaInicio, fechaFin);
            ViewBag.RespuestasPrecios = accesoDatos.ObtenerCantidadRespuestasPorPreguntaYFecha(4, fechaInicio, fechaFin);
            ViewBag.RespuestasSatisfecho = accesoDatos.ObtenerCantidadRespuestasPorPreguntaYFecha(5, fechaInicio, fechaFin);

            ViewBag.Comentarios = accesoDatos.ObtenerComentariosDeCuestionarioPorFecha("Califica tu experiencia", fechaInicio, fechaFin);

            ObtenerResumenesEvaluaciones(fechaInicio, fechaFin);

            ViewBag.Fechas = "Entre el " + fechaInicio + " y " + fechaFin + " se obtienen los siguientes resultados:";

            return View();
        }
    }
}