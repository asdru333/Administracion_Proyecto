using System;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using Planetario.Handlers;
using Planetario.Models;
using System.Collections.Generic;
using Planetario.Interfaces;
using System.Diagnostics;

namespace Planetario.Controllers
{
    public class ConcursoController : Controller
    {

        private readonly ConcursoInterfaz AccesoDatos;
        readonly CookiesInterfaz cookiesInterfaz;
        public ConcursoController()
        {
            AccesoDatos = new ConcursoHandler();
            cookiesInterfaz = new CookiesHandler();
        }

        public ConcursoController(ConcursoInterfaz service, CookiesInterfaz cookies)
        {
            AccesoDatos = service;
            cookiesInterfaz = cookies;
        }

        [HttpGet]
        public ActionResult CrearConcurso()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CrearConcurso(ConcursoModel concurso)
        {
            ViewBag.ExitoAlCrear = false;
            try
            {
                if (ModelState.IsValid)
                {

                    ViewBag.ExitoAlCrear = AccesoDatos.InsertarConcurso(concurso);
                    if (ViewBag.ExitoAlCrear)
                    {
                        ViewBag.Message = "El concurso " + concurso.NombreConcurso + " fue creado con éxito.";
                        ModelState.Clear();
                    }
                    else
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
                ViewBag.Message = "Hubo un error al crear el cuestionario " + concurso.NombreConcurso;
                return View();
            }
        }

        [HttpGet]
        public ActionResult ListadoDeConcursos()
        {
            ViewBag.concursos = AccesoDatos.ObtenerConcursosAbiertos(1);
            return View();
        }

        [HttpPost]
        public ActionResult ListadoDeConcursos(string opcionFiltro = "todos")
        {
            if (opcionFiltro == "Abiertos")
                ViewBag.concursos = AccesoDatos.ObtenerConcursosAbiertos(1);
            else if (opcionFiltro == "Cerrados")
                ViewBag.concursos = AccesoDatos.ObtenerConcursosAbiertos(0);
            else if (opcionFiltro == "Ganadores")
                ViewBag.concursos = AccesoDatos.ObtenerConcursosConGanadorDeclarado();
            else if (opcionFiltro == "Mis concursos")
            {
                string correoUsuario = cookiesInterfaz.CorreoUsuario();
                ViewBag.concursos = AccesoDatos.ObtenerConcursosPersona(correoUsuario);
            }
            else
                ViewBag.concursos = AccesoDatos.ObtenerTodosLosConcursos();
            return View();
        }

        [HttpGet]
        public ActionResult VerConcurso(string concurso)
        {
            ViewBag.concurso = AccesoDatos.ObtenerConcurso(concurso);
            ViewBag.sesion = AccesoDatos.TieneInicioSesion();
            ViewBag.inscrito = AccesoDatos.EstaInscrito(concurso);
            return View();
        }

        [HttpGet]
        public ActionResult AdministrarConcursos()
        {
            ViewBag.concursos = AccesoDatos.ObtenerTodosLosConcursos();
            return View();
        }

        [HttpPost]
        public ActionResult AdministrarConcursos(string opcionFiltro = "todos")
        {
            if (opcionFiltro == "Abiertos")
                ViewBag.concursos = AccesoDatos.ObtenerConcursosAbiertos(1);
            else if (opcionFiltro == "Cerrados")
                ViewBag.concursos = AccesoDatos.ObtenerConcursosAbiertos(0);
            else if (opcionFiltro == "Ganadores")
                ViewBag.concursos = AccesoDatos.ObtenerConcursosConGanadorDeclarado();
            else if (opcionFiltro == "Mis concursos")
            {
                string correoUsuario = cookiesInterfaz.CorreoUsuario();
                ViewBag.concursos = AccesoDatos.ObtenerConcursosPersona(correoUsuario);
            }
            else
                ViewBag.concursos = AccesoDatos.ObtenerTodosLosConcursos();
            return View();
        }

        [HttpGet]
        public ActionResult ListaParticipantes(string nombreDelConcurso)
        {
            ViewBag.participantes = AccesoDatos.ObtenerParticipantes(nombreDelConcurso);
            ViewBag.nombreConcurso = nombreDelConcurso;
            return View();
        }

        [HttpGet]
        public ActionResult DeclararGanador(string nombreDelConcurso, string correoParticipante)
        {
            AccesoDatos.InsertarGanador(nombreDelConcurso, correoParticipante);
            return RedirectToAction("AdministrarConcursos");
        }

        [HttpGet]
        public ActionResult EliminarConcurso(string nombreDelConcurso)
        {
            AccesoDatos.EliminarConcurso(nombreDelConcurso);
            return RedirectToAction("AdministrarConcursos");
        }

        [HttpGet]
        public ActionResult ActualizarConcurso(string nombreDelConcurso)
        {
            ConcursoModel concurso = AccesoDatos.ObtenerConcurso(nombreDelConcurso);
            concurso.Fecha = CambiarFormatoFecha(concurso.Fecha);
            ViewBag.concurso = concurso;
            return View();
        }

        [HttpPost]
        public ActionResult ActualizarConcurso(ConcursoModel concurso)
        {
            try
            {
                ViewBag.concurso = concurso;
                if (ModelState.IsValid)
                {
                    if (AccesoDatos.ActualizarConcurso(concurso))
                    {
                        ViewBag.Message = "El concurso " + concurso.NombreConcurso + " fue actualizado con éxito.";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Message = "Hubo un error al guardar los datos ingresados.";
                    }
                }
                else
                {
                    ViewBag.Message = "Hay un error en los datos ingresados";
                }
            } catch
            {
                ViewBag.Message = "Hubo un error al actualizar el concurso " + concurso.NombreConcurso;
            }
            return View();
        }

        [HttpGet]
        public ActionResult CerrarConcurso(string nombreDelConcurso)
        {
            AccesoDatos.CerrarConcurso(nombreDelConcurso);
            return RedirectToAction("AdministrarConcursos");
        }

        [HttpGet]
        public ActionResult inscribirse(string nombreDelConcurso)
        {
            AccesoDatos.Inscribirse(nombreDelConcurso);
            return RedirectToAction("VerConcurso", new {concurso = nombreDelConcurso });
        }

        [HttpGet]
        public ActionResult Desinscribirse(string nombreDelConcurso)
        {
            bool lol = AccesoDatos.Desinscribirse(nombreDelConcurso);
            return RedirectToAction("VerConcurso", new { concurso = nombreDelConcurso });
        }

        public string CambiarFormatoFecha(string fecha)
        {
            fecha = fecha.Substring(0, fecha.IndexOf(' '));
            string[] valores = fecha.Split('/');
            if (valores[0].Length == 1)
            {
                valores[0] = '0' + valores[0];
            }
            if (valores[1].Length == 1)
            {
                valores[1] = '0' + valores[1];
            }
            return fecha = valores[2] + "-" + valores[1] + "-" + valores[0];
        }
    }
}