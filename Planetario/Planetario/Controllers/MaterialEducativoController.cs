using System.Web.Mvc;
using Planetario.Handlers;
using Planetario.Models;

namespace Planetario.Controllers
{
    public class MaterialEducativoController : Controller
    {
        public ActionResult AlmacenarNuevoMaterialEducativo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AlmacenarNuevoMaterialEducativo(MaterialEducativoModel MaterialEducativo)
        {
            ViewBag.ExitoAlmacenar = false;
            try
            {
                if (ModelState.IsValid)
                {
                    MaterialesEducativosHandler AccesoADatos = new MaterialesEducativosHandler();
                    ViewBag.ExitoAlmacenar = AccesoADatos.AlmacenarMaterialEducativo(MaterialEducativo);
                    if(ViewBag.ExitoAlmacenar)
                    {
                        ViewBag.Mensaje = "El material " + MaterialEducativo.Titulo + " fue almacenado con exito 😉";
                        ModelState.Clear();
                    } 
                    else
                    {
                        ViewBag.Mensaje = "Hubo un error en la base de datos ☹";
                    }
                }
                else
                {
                    ViewBag.Mensaje = "El modelo no es valido!";
                }
                return View();
            }
            catch
            {
                ViewBag.Mensaje = "Algo salió mal y no fue posible almacenar el material 😐";
                return View();
            }
        }

        public ActionResult listadoDeMateriales()
        {
            MaterialesEducativosHandler accesoDatos = new MaterialesEducativosHandler();
            ViewBag.materiales = accesoDatos.obtenerMateriales();
            return View();
        }

        [HttpGet]
        public FileResult descargarArchivo(string titulo)
        {
            MaterialesEducativosHandler accesoDatos = new MaterialesEducativosHandler();
            var tupla = accesoDatos.descargarContenido(titulo);
            return File(tupla.Item1, tupla.Item2);
        }

        public ActionResult verMaterial(string nombre)
        {
            MaterialesEducativosHandler accesoDatos = new MaterialesEducativosHandler();
            ViewBag.material = accesoDatos.buscarActividad(nombre);
            ViewBag.materiales = accesoDatos.obtenerTodasLosMaterialesRecomendados(ViewBag.material.PublicoDirigido, ViewBag.material.Categoria);
            return View();
        }

        [HttpGet]
        public FileResult descargarVistaPrevia(string titulo)
        {
            MaterialesEducativosHandler accesoDatos = new MaterialesEducativosHandler();
            var tupla = accesoDatos.descargarVistaPrevia(titulo);
            return File(tupla.Item1, tupla.Item2);
        }

        [HttpGet]
        public ActionResult buscarMaterialesEducativos(string palabra)
        {
            MaterialesEducativosHandler accesoDatos = new MaterialesEducativosHandler();
            ViewBag.materialesUnicos = accesoDatos.obtenerMaterialBuscado(palabra);
            return View();
        }


    }
}