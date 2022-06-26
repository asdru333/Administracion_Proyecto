using System;
using System.Web.Mvc;
using Planetario.Handlers;
using Planetario.Models;
using System.Collections.Generic;
using Planetario.Interfaces;

namespace Planetario.Controllers
{
    public class ConcursoController : Controller
    {

        private readonly ConcursoInterfaz AccesoDatos;

        public ConcursoController()
        {
            AccesoDatos = new ConcursoHandler();
        }

        public ConcursoController(ConcursoInterfaz service)
        {
            AccesoDatos = service;
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
            ViewBag.concursos = AccesoDatos.ObtenerConcursosInscribibles(1);
            return View();
        }

        [HttpGet]
        public ActionResult VerConcurso(string concurso)
        {
            ViewBag.concurso = AccesoDatos.ObtenerConcurso(concurso);
            return View();
        }

        [HttpGet]
        public ActionResult AdministrarConcursos()
        {
            ViewBag.concursos = AccesoDatos.ObtenerTodosLosConcursos();
            return View();
        }

        [HttpGet]
        public ActionResult EliminarConcurso(string concurso)
        {
            //AccesoDatos.EliminarConcurso(concurso);
            return RedirectToAction("AdministrarConcursos");
        }

        [HttpGet]
        public ActionResult CerrarConcurso(string nombreDelConcurso)
        {
            AccesoDatos.CerrarConcurso(nombreDelConcurso);
            return RedirectToAction("AdministrarConcursos");
        }
    }
}