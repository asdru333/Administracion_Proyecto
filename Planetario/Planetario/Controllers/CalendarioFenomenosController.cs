using Planetario.Models;
using Planetario.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Planetario.Controllers
{
    public class CalendarioController : Controller
    {
        public ActionResult CalendarioFenomenos()
        {         
            return View();
        }
        
        public JsonResult GetActividadesPlanetario()
        {
            List<object> resultado = new List<object>();
            ActividadHandler accesoDatos = new ActividadHandler();
            List<ActividadModel> actividades = accesoDatos.ObtenerActividadesAprobadas();
            
            foreach (ActividadModel actividad in actividades)
            {
                resultado.Add( new {
                    title = actividad.NombreActividad,
                    start = TranslateFecha(actividad.Fecha),
                    url = Url.Action("verActividad", "Actividades", new { nombre = actividad.NombreActividad })
                });
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEventosPlanetario()
        {
            List<object> resultado = new List<object>();
            EventosHandler accesoDatos = new EventosHandler();
            List<EventoModel> eventos = accesoDatos.ObtenerTodosEventos();

            foreach (EventoModel evento in eventos)
            {
                resultado.Add(new {
                    title = evento.Titulo,
                    start = TranslateFecha(evento.Fecha),
                    url = Url.Action("VerEvento", "Eventos", new { titulo = evento.Titulo })
                });
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEventosRSSFeed()
        {
            List<object> resultado = new List<object>();
            XDocument xml = XDocument.Load("https://in-the-sky.org//rss.php?feed=dfan&latitude=9.93333&longitude=-84.08333&timezone=America/Costa_Rica");
            var RSSFeedData = (from x in xml.Descendants("item")
                               select new RSSFeedModel
                               {
                                   Title = splitTitulo(((string)x.Element("title"))),
                                   Link = ((string)x.Element("link")),
                                   Description = ((string)x.Element("description")),
                                   PubDate = TranslateFecha(((string)x.Element("pubDate")))
                               });
            foreach (RSSFeedModel evento in RSSFeedData)
            {
                resultado.Add(new
                {
                    title = evento.Title,
                    start = evento.PubDate,
                    description = evento.Description,
                    url = evento.Link,
                    allDay = true,
                });
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        //Input: Sat, 06 Nov 2021 17:22:13 GMT
        //Output: 2021-11-6
        public string TranslateFecha(string fecha)
        {
            DateTime fechaDateTime = DateTime.Parse(fecha);
            string year = AppendRestante(fechaDateTime.Year.ToString(), fechaDateTime.Year);
            string month = AppendRestante(fechaDateTime.Month.ToString(), fechaDateTime.Month);
            string day = AppendRestante(fechaDateTime.Day.ToString(), fechaDateTime.Day);

            string resultado = year + '-' + month + '-' + day;
            return resultado;
        }

        public string AppendRestante(string toAppend, int comprobacion)
        {
            if (comprobacion < 10){ 
            toAppend = "0" + toAppend;
            }
            return toAppend;
        }

        public string splitTitulo(string titulo)
        {
            string[] words = titulo.Split(':');
            return words[1];
        }
    }
}