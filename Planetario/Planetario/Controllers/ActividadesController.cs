using System;
using System.Web.Mvc;
using Planetario.Handlers;
using Planetario.Models;
using System.Collections.Generic;
using Planetario.Interfaces;

namespace Planetario.Controllers
{
    public class ActividadesController : Controller
    {

        private readonly ActividadesInterfaz AccesoDatos;

        public ActividadesController()
        {
            AccesoDatos = new ActividadHandler();
        }

        public ActividadesController(ActividadesInterfaz service)
        {
            AccesoDatos = service;
        }

        public ActionResult CrearActividad()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CrearActividad(ActividadModel actividad, string topicos)
        {
            ViewBag.ExitoAlCrear = false;
            string[] topicosSeleccionados = topicos.Split(';');
            try
            {
                if (ModelState.IsValid)
                {
                    
                    ViewBag.ExitoAlCrear = AccesoDatos.InsertarActividad(actividad);
                    if (ViewBag.ExitoAlCrear)
                    {
                        ViewBag.Message = "La actividad " + actividad.NombreActividad + " fue creada con éxito.";
                        foreach(string topico in topicosSeleccionados)
                        {
                            AccesoDatos.InsertarTopico(actividad.NombreActividad, topico);
                        }
                        ModelState.Clear();
                    } else
                    {
                        ViewBag.Message = "Hubo un error al guardar los datos ingresados.";
                    }
                }
                else
                {
                    ViewBag.Message = "Hay un error en los datos ingresados";
                }
                return View();
            }
            catch
            {
                ViewBag.Message = "Hubo un error al crear el cuestionario " + actividad.NombreActividad;
                return View();
            }
        }

        public ActionResult ListadoDeActividades()
        {

            ViewBag.actividades = AccesoDatos.ObtenerActividadesAprobadas();
            return View();
        }

        public ActionResult VerActividad(string nombreDeLaActividad)
        {
            
            ViewBag.actividad = AccesoDatos.ObtenerActividad(nombreDeLaActividad);
            ViewBag.topicos = AccesoDatos.ObtenerTopicosActividad(nombreDeLaActividad);
            ViewBag.actividades = AccesoDatos.ObtenerActividadesRecomendadas(ViewBag.actividad.PublicoDirigido, ViewBag.actividad.Complejidad);
            ViewBag.entradasDisponibles = AccesoDatos.ObtenerEntradasDisponiblesPorActividad(nombreDeLaActividad);
            ViewBag.asientosRelacionadosActividad = AccesoDatos.ObtenerAsientos(nombreDeLaActividad);
            return View();
        }

        [HttpGet]
        public ActionResult ComprarEntradas(string nombreDeLaActividad)
        {
            ViewBag.actividad = AccesoDatos.ObtenerActividad(nombreDeLaActividad);
            ViewBag.topicos = AccesoDatos.ObtenerTopicosActividad(nombreDeLaActividad);
            ViewBag.actividades = AccesoDatos.ObtenerActividadesRecomendadas(ViewBag.actividad.PublicoDirigido, ViewBag.actividad.Complejidad);
            ViewBag.entradasDisponibles = AccesoDatos.ObtenerEntradasDisponiblesPorActividad(nombreDeLaActividad);
            ViewBag.asientosRelacionadosActividad = AccesoDatos.ObtenerAsientos(nombreDeLaActividad);
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerAsientosReservados(string nombreDeLaActividad)
        {
            
            List<AsientoModel> asientosRelacionadosActividad = AccesoDatos.ObtenerAsientos(nombreDeLaActividad);
            List<int> reservados = new List<int>();
            for (int i = 0; i < asientosRelacionadosActividad.Count; i++)
            {
                AsientoModel asiento = asientosRelacionadosActividad[i];
                if (asiento.Reservado || asiento.Vendido)
                {
                    reservados.Add(i);
                }
            }
            return Json(reservados, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BuscarActividad()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BuscarActividad(string palabra)
        {
            
            ViewBag.actividadesUnicas = AccesoDatos.ObtenerActividadesPorBusqueda(palabra);
            return View();
        }

        [HttpGet]
        public ActionResult Inscribirme(string titulo)
        {
            
            DatosHandler datosHandler = new DatosHandler();
            ViewBag.paises = datosHandler.SelectListPaises();
            ViewBag.nivelesEducativos = datosHandler.SelectListNivelesEducativos();
            ViewBag.generos = datosHandler.SelectListGeneros();
            ViewBag.precio = AccesoDatos.ObtenerActividad(titulo).PrecioAproximado;
            ViewBag.titulo = titulo;
            return View();
        }

        [HttpPost]
        public ActionResult Inscribirme(InscripcionModel info)
        {
            ViewBag.exitoAlInscribir = false;
            try
            {
                if (ModelState.IsValid)
                {
                    ParticipanteHandler AccesoDatos = new ParticipanteHandler();
                    ActividadHandler actividad = new ActividadHandler();
                    bool estaAlmacenado = AccesoDatos.ParticipanteEstaAlmacenado(info.infoParticipante.correo);
                    if (!estaAlmacenado)
                        estaAlmacenado = AccesoDatos.AlmacenarParticipante(info.infoParticipante);
                    if (estaAlmacenado)
                        ViewBag.exitoAlInscribir = AccesoDatos.AlmacenarParticipacion(info.infoParticipante.correo, Request.Form["TituloActividad"], Double.Parse(Request.Form["PrecioActividad"]));

                    if (ViewBag.exitoAlInscribir)
                    {
                        ViewBag.Message = "Usted ha está inscrito en la actividad " + Request.Form["TituloActividad"];
                        ModelState.Clear();
                    }
                }
                return View();
            }
            catch
            {
                ViewBag.Message = "Algo salió mal.";
                return View();
            }
        }

        [HttpGet]
        public ActionResult VerFacturasDeActividad(string actividadNombre)
        {
            FacturasHandler AccesoDatos = new FacturasHandler();
            ViewBag.facturas = AccesoDatos.ObtenerFacturasDeActividad(actividadNombre);
            ViewBag.nombreActividad = actividadNombre;
            return View();
        }

        [HttpGet]
        public JsonResult ValidarCorreo(string correo)
        {
            ParticipanteHandler AccesoDatos = new ParticipanteHandler();
            bool existe = AccesoDatos.ParticipanteEstaAlmacenado(correo);
            if (existe)
            {
                var participante = AccesoDatos.ObtenerParticipante(correo);
                return Json(new{estaAlmacenado = true, infoPersonal = participante}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new {estaAlmacenado = false, infoPersonal = ""}, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpGet]
        public JsonResult ActualizarReservacionAsiento(int fila, int columna, string correo, bool reservado, string nombreActividad)
        {
            ActividadHandler accesoDatos = new ActividadHandler();
            bool exito = accesoDatos.ActualizarReservarAsiento(fila, columna, correo, reservado, nombreActividad);
            return Json(new {Exito = exito }, JsonRequestBehavior.AllowGet);
        }
    }
}