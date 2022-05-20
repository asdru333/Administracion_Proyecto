using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using Planetario.Interfaces;

namespace Planetario.Handlers
{
    public class DatosHandler : DatosInterfaz
    {
        public List<string> paises;
        public List<string> generos;
        public List<string> complejidades;
        public List<string> publicos;
        public List<string> tiposDeActividad;
        public List<string> categorias;
        public List<string> diasDeLaSemana;
        public List<string> nivelesEducativos;
        public Dictionary<string, List<string>> topicosPorCategoria;
        public List<string> evaluacion;

        public DatosHandler()
        {
            paises = CargarListaDeStringsDesdeArchivo("Paises.json");
            generos = CargarListaDeStringsDesdeArchivo("Generos.json");
            complejidades = CargarListaDeStringsDesdeArchivo("Complejidades.json");
            publicos = CargarListaDeStringsDesdeArchivo("Publicos.json");
            tiposDeActividad = CargarListaDeStringsDesdeArchivo("TiposDeActividad.json");
            diasDeLaSemana = CargarListaDeStringsDesdeArchivo("DiasDeLaSemana.json");
            nivelesEducativos = CargarListaDeStringsDesdeArchivo("NivelesEducativos.json");
            evaluacion = CargarListaDeStringsDesdeArchivo("OpcionesEvaluacion.json");
            CargarCategoriasYTopicosDesdeArchivo();
        }

        public string ObtenerRutaDocumento(string nombreDocumento)
        {
            return (Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "App_Data", nombreDocumento));
        }

        private List<string> CargarListaDeStringsDesdeArchivo(string nombreDocumento)
        {
            string jsonString = File.ReadAllText(ObtenerRutaDocumento(nombreDocumento));
            List<string> lista = JsonConvert.DeserializeObject<List<string>>(jsonString);
            return (lista);
        }

        private void CargarCategoriasYTopicosDesdeArchivo()
        {
            string jsonCategorias = File.ReadAllText(ObtenerRutaDocumento("CategoriasYTopicos.json"));
            topicosPorCategoria = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonCategorias);
            categorias = new List<string>();
            foreach(KeyValuePair<string,List<string>> categoria in topicosPorCategoria)
            {
                categorias.Add(categoria.Key);
            }
        }

        public List<string> ObtenerTodosLosTopicos()
        {
            List<string> topicos = new List<string>();
            foreach(string categoria in categorias)
            {
                foreach(string topico in topicosPorCategoria[categoria])
                {
                    topicos.Add(topico);
                }
            }
            topicos.Sort();
            return topicos;
        }

        public List<string> ObtenerTopicosPorCategoria(string categoria)
        {
            List<string> topicos = new List<string>();
            foreach(string topico in topicosPorCategoria[categoria])
            {
                topicos.Add(topico);
            }
            topicos.Sort();
            return topicos;
        }

        public List<SelectListItem> CrearSelectList(List<string> listaDeStrings)
        {
            List<SelectListItem> lista = new List<SelectListItem>();
            foreach (string item in listaDeStrings)
            {
                lista.Add(new SelectListItem { Text = item, Value = item });
            }
            return lista;
        }

        public List<SelectListItem> SelectListPaises()
        {
            return (CrearSelectList(paises));
        }
        public List<SelectListItem> SelectListCategorias()
        {
            return (CrearSelectList(categorias));
        }
        public List<SelectListItem> SelectListGeneros()
        {
            return (CrearSelectList(generos));
        }
        public List<SelectListItem> SelectListComplejidades()
        {
            return (CrearSelectList(complejidades));
        }
        public List<SelectListItem> SelectListPublicos()
        {
            return (CrearSelectList(publicos));
        }
        public List<SelectListItem> SelectListTiposDeActividad()
        {
            return (CrearSelectList(tiposDeActividad));
        }
        public List<SelectListItem> SelectListNivelesEducativos()
        {
            return (CrearSelectList(nivelesEducativos));
        }
        public List<SelectListItem> SelectListDiasDeLaSemana()
        {
            return (CrearSelectList(diasDeLaSemana));
        }
        public List<SelectListItem> SelectListTodosLosTopicos()
        {
            return (CrearSelectList(ObtenerTodosLosTopicos()));
        }
    }
}