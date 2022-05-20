using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Planetario.Handlers;
using Planetario.Models;

namespace Planetario.Controllers
{
    public class FuncionariosController : Controller
    {
        public ActionResult ListaFuncionarios()
        {
            FuncionariosHandler AccesoDatos = new FuncionariosHandler();
            ViewBag.ListaFuncionarios = AccesoDatos.ObtenerTodosLosFuncionarios();
            return View();
        }

        [HttpGet]
        public ActionResult ObtenerImagen(string correo)
        {
            FuncionariosHandler productHandler = new FuncionariosHandler();
            var tupla = productHandler.ObtenerFoto(correo);
            return File(tupla.Item1, tupla.Item2);
        }


        public ActionResult VerFuncionario(string correo)
        {
            FuncionariosHandler AccesoDatos = new FuncionariosHandler();
            ViewBag.Funcionario = AccesoDatos.ObtenerFuncionario(correo);
            ViewBag.Idiomas = AccesoDatos.ObtenerIdiomasFuncionario(correo);
            ViewBag.Titulos = AccesoDatos.ObtenerTitulosFuncionario(correo);
            ViewBag.Roles = AccesoDatos.ObtenerRolesFuncionario(correo);

            return View();
        }

        [HttpGet]
        public ActionResult AgregarFuncionario()
        {
            DatosHandler dataHandler = new DatosHandler();
            ViewBag.paises = dataHandler.SelectListPaises();
            ViewBag.generos = dataHandler.SelectListGeneros();
            ViewBag.opcionIdiomas = obtenerIdiomas();
            return View();
        }

        [HttpPost]
        public ActionResult AgregarFuncionario(FuncionarioModel funcionario, string idiomas)
        {
            List<SelectListItem> opcionIdiomas = new List<SelectListItem>();
            opcionIdiomas = obtenerIdiomas();
            ViewBag.opcionIdiomas = opcionIdiomas;

            ViewBag.ExitoAlCrear = false;
            try
            {
                if (ModelState.IsValid)
                {
                    FuncionariosHandler accesoDatos = new FuncionariosHandler();

                    string[] idioma = idiomas.Split(';');
                    List<string> idiomasSelect = new List<string>(idioma);

                    ViewBag.ExitoAlCrear = accesoDatos.InsertarFuncionario(funcionario);
                    if (ViewBag.ExitoAlCrear)
                    {
                        ViewBag.Message = "El funcionario "  + funcionario.nombre + " fue agregado con éxito.";
                        foreach(var variable in idiomasSelect)
                        {
                            accesoDatos.InsertarIdiomas(variable, funcionario.correo);
                        }
                        ModelState.Clear();
                    }
                }
                return View();
            }
            catch
            {
                ViewBag.Message = "Algo salió mal ";
                return View();
            }
        }

        public List<SelectListItem> obtenerIdiomas()
        {
            EstadisticasHandler accesoDatos = new EstadisticasHandler();

            var idiomas = accesoDatos.obtenerListaIdiomas();


            List<SelectListItem> listaIdiomas = new List<SelectListItem>();
            string todos = "Todos";

            for (int i = 0; i < idiomas.Count(); i++)
            {
                listaIdiomas.Add(new SelectListItem()
                {
                    Text = idiomas[i],
                    Selected = (idiomas[i] == todos ? true : false)
                });
            }

            return listaIdiomas;
        }
    }
}