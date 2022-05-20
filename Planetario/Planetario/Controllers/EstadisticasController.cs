using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Planetario.Handlers;
using Planetario.Models;

namespace Planetario.Controllers
{
    public class EstadisticasController : Controller
    {

        [HttpGet]
        public ActionResult verInvolucramiento()
        {
            llenarListasParticipacion();
            return View();
        }

        [HttpPost]
        public ActionResult verInvolucramiento(string opcionDia, string opcionPublico, string opcionComplejidad, string opcionCategoria, string opcionTopico)
        {
            EstadisticasHandler accesoDatos = new EstadisticasHandler();
            int cantidadTotalParticipantes = accesoDatos.obtenerCantidadDeParticipantes(opcionDia, opcionPublico, opcionComplejidad, opcionCategoria, opcionTopico);
            ViewBag.Mensaje = stringResultado(opcionDia, opcionPublico, opcionComplejidad, opcionCategoria, opcionTopico, cantidadTotalParticipantes);
            llenarListasParticipacion();
            return View();
        }

        public void llenarListasParticipacion()
        {
            EstadisticasHandler accesoDatos = new EstadisticasHandler();
            DatosHandler datosHandler = new DatosHandler();

            ViewBag.listaPublico = datosHandler.SelectListPublicos();
            ViewBag.listaDias = datosHandler.SelectListDiasDeLaSemana();
            ViewBag.listaComplejidad = datosHandler.SelectListComplejidades();
            ViewBag.listaCategorias = datosHandler.SelectListCategorias();
            ViewBag.listaTopicos = datosHandler.SelectListTodosLosTopicos();

            // Gráficos

            List<int> listaParticipacionesPorDia = new List<int>();
            List<int> listaParticipacionesPorComplejidad = new List<int>();
            List<int> listaParticipacionesPorPublico = new List<int>();
            List<int> listaParticipacionesPorCategoria = new List<int>();
            List<int> listaParticipacionesPorTopicoTodos = new List<int>();

            foreach (var dia in ViewBag.listaDias)
            {
                listaParticipacionesPorDia.Add(accesoDatos.obtenerCantidadDeParticipantes(dia.Text, "", "", "", ""));
                //listaParticipacionesPorDia.Add(accesoDatos.ContarParticipantesPorDia(dia.Text));
            }
            foreach (var publico in ViewBag.listaPublico)
            {
                listaParticipacionesPorPublico.Add(accesoDatos.obtenerCantidadDeParticipantes("", publico.Text, "", "", ""));
                //listaParticipacionesPorPublico.Add(accesoDatos.ContarParticipantesPorPublico(publico.Text));
            }
            foreach (var complejidad in ViewBag.listaComplejidad)
            {
                listaParticipacionesPorComplejidad.Add(accesoDatos.obtenerCantidadDeParticipantes("", "", complejidad.Text, "", ""));
                //listaParticipacionesPorComplejidad.Add(accesoDatos.ContarParticipantesPorComplejidad(complejidad.Text));
            }
            foreach (var categoria in ViewBag.listaCategorias)
            {
                listaParticipacionesPorCategoria.Add(accesoDatos.obtenerCantidadDeParticipantes("", "", "", categoria.Text, ""));
                //listaParticipacionesPorCategoria.Add(accesoDatos.ContarParticipantesPorCategoria(categoria.Text));
            }
            foreach (var topico in ViewBag.listaTopicos)
            {
                listaParticipacionesPorTopicoTodos.Add(accesoDatos.obtenerCantidadDeParticipantes("", "", "", "", topico.Text));
                //listaParticipacionesPorTopicoTodos.Add(accesoDatos.ContarParticipantesPorTopico(topico.Text));
            }

            ViewBag.participacionesPorDia = listaParticipacionesPorDia;
            ViewBag.participacionesPorPublico = listaParticipacionesPorPublico;
            ViewBag.participacionesPorComplejidad = listaParticipacionesPorComplejidad;
            ViewBag.participacionesPorCategoria = listaParticipacionesPorCategoria;
            ViewBag.participacionesPorTopicoTodos = listaParticipacionesPorTopicoTodos;
        }

        [HttpGet]
        public ActionResult verIdiomas()
        {
            EstadisticasHandler accesoDatos = new EstadisticasHandler();

            List<string> listaDeIdiomas = accesoDatos.obtenerListaIdiomas();
            List<int> listaNumIdiomas = new List<int>();
            List<SelectListItem> opcionIdiomas = new List<SelectListItem>();

            opcionIdiomas = obtenerIdiomas();
       
            foreach (var idioma in listaDeIdiomas)
            {
                listaNumIdiomas.Add(accesoDatos.obtenerNumIdiomas(idioma));
            }

            ViewBag.listaIdiomas = listaDeIdiomas;
            ViewBag.listaNumIdiomas = listaNumIdiomas;
            ViewBag.funcionariosBuscados = "";
            ViewBag.opcionIdiomas = opcionIdiomas;

            return View();
        }

        [HttpPost]
        public ActionResult verIdiomas(string seleccionIdiomas)
        {

            EstadisticasHandler accesoDatos = new EstadisticasHandler();

            string[] idiomasSeleccionados = seleccionIdiomas.Split(';');

            List<string> idiomas = new List<string>(idiomasSeleccionados);
            List<string> listaDeIdiomas = accesoDatos.obtenerListaIdiomas();
            List<SelectListItem> opcionIdiomas = new List<SelectListItem>();
            List<int> listaNumIdiomas = new List<int>();
            List<EstadisticasModel> listaFuncionarios = new List<EstadisticasModel>();

            opcionIdiomas = obtenerIdiomas();
            listaFuncionarios = accesoDatos.obtenerFuncionarios(idiomas);

            foreach (var idioma in listaDeIdiomas)
            {
                listaNumIdiomas.Add(accesoDatos.obtenerNumIdiomas(idioma));
            }

            ViewBag.listaIdiomas = listaDeIdiomas;
            ViewBag.listaNumIdiomas = listaNumIdiomas;
            ViewBag.opcionIdiomas = opcionIdiomas;
            ViewBag.funcionariosBuscados = listaFuncionarios;
            ViewBag.cantidad = listaFuncionarios.Count();
            ViewBag.cantidadFuncionarios = stringResultadoIdiomas(listaFuncionarios, idiomas);

            return View();
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

        public string stringResultado(string dia, string publico, string complejidad, string categoria, string topico, int total)
        {
            string mensaje = "";

            mensaje = concatenarDia(mensaje, dia);
            mensaje = concatenarPublico(mensaje, publico);
            mensaje = concatenarComplejidad(mensaje, complejidad);
            mensaje = concatenarCategoria(mensaje, categoria);
            mensaje = concatenarTopico(mensaje, topico);
            mensaje = concatenarTotal(mensaje, total);

            return mensaje;
        }

        public string concatenarDia(string resultado, string dia) {

            if (dia == "")
            {
                resultado += "Todos los días, ";
            }
            else
            {
                resultado += "El día " + dia;
            }

            return resultado;
        }

        public string concatenarPublico(string resultado, string publico)
        {

            if (publico == "")
            {
                resultado += " con todos los públicos, ";
            }
            else
            {
                resultado += " con el público " + publico + ", ";
            }

            return resultado;
        }

        public string concatenarComplejidad(string resultado, string complejidad)
        {

            if (complejidad == "")
            {
                resultado += "todas las complejidades, ";
            }
            else
            {
                resultado += "la complejidad " + complejidad + ", ";
            }

            return resultado;
        }

        public string concatenarCategoria(string resultado, string categoria)
        {

            if (categoria == "")
            {
                resultado += "todas las categorias";
            }
            else
            {
                resultado += "la categoria " + categoria;
            }

            return resultado;
        }

        public string concatenarTopico(string resultado, string topico)
        {

            if (topico == "")
            {
                resultado += " y todos los topicos hay: ";
            }
            else
            {
                resultado += " y el topico " + topico + " hay: ";
            }

            return resultado;
        }

        public string concatenarTotal(string resultado, int total)
        {

            if (total != 1)
            {
                resultado += total + " participantes";
            }
            else
            {
                resultado += total + " participante";
            }

            return resultado;
        }

        public string stringResultadoIdiomas(List<EstadisticasModel> listaFuncionarios, List<string> idiomas)
        {
            string mensaje = "";
            int resultado = listaFuncionarios.Count();

            if(resultado > 1)
            {
                mensaje += "Se encontraron " + resultado + " funcionarios que hablan: ";

                for (int i = 0; i < idiomas.Count()-2; i++)
                {
                    mensaje += idiomas[i] + ", ";
                }

                mensaje += idiomas[idiomas.Count()-2];
            }

            if (resultado == 1)
            {
                mensaje += "Se encontró " + resultado + " funcionario que habla: ";

                for (int i = 0; i < idiomas.Count()-2; i++)
                {
                    mensaje += idiomas[i] + ", ";
                }

                mensaje += idiomas[idiomas.Count()-2];
            }

            if (resultado == 0)
            {
                mensaje += "Se encontraron " + resultado + " funcionarios que hablan: ";

                for (int i = 0; i < idiomas.Count() - 2; i++)
                {
                    mensaje += idiomas[i] + ", ";
                }

                mensaje += idiomas[idiomas.Count() - 2];
            }

            return mensaje;
        }

    }
}