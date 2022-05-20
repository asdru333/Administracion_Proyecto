using System.Collections.Generic;
using System.Web.Mvc;
using Planetario.Handlers;
using Planetario.Models;

namespace Planetario.Controllers
{
    public class NoticiasController : Controller
    {
        public ActionResult listadoDeNoticias()
        {
            NoticiasHandler accesoDatos = new NoticiasHandler();
            ViewBag.noticias = accesoDatos.ObtenerTodasLasNoticias();
            return View();
        }

        public ActionResult crearNoticia()
        {
            return View();
        }

        [HttpPost]
        public ActionResult crearNoticia(NoticiaModel noticia)
        {
            ViewBag.ExitoAlCrear = false;
            try
            {
                if (ModelState.IsValid)
                {
                    NoticiasHandler accesoDatos = new NoticiasHandler();
                    ViewBag.ExitoAlCrear = accesoDatos.InsertarNoticia(noticia);
                    if (ViewBag.ExitoAlCrear)
                    {
                        ViewBag.Message = "La noticia" + " " + noticia.Titulo + " fue creada con éxito :)";
                        ModelState.Clear();
                    }
                }
                return View();
            }
            catch
            {
                ViewBag.Message = "Algo salió mal y no fue posible crear la noticia :(";
                return View();
            }
        }

        public ActionResult Topicos(string categoria)
        {
            switch (categoria)
            {
                case "Cuerpos del Sistema Solar":
                    return Json(
                    new List<SelectListItem>() {
                    new SelectListItem { Text = "-Topico-", Value = null},
                    new SelectListItem { Text = "Planetas", Value = "Planetas" },
                    new SelectListItem { Text = "Satelites", Value = "Satelites"  },
                    new SelectListItem { Text = "Cometas", Value = "Cometas"  },
                    new SelectListItem { Text = "Asteroides", Value = "Asteroides"  }
                    },
                    JsonRequestBehavior.AllowGet
                    );
                case "Objetos de Cielo Profundo":
                    return Json(
                    new List<SelectListItem>() {
                    new SelectListItem { Text = "-Topico-", Value = null},
                    new SelectListItem { Text = "Galaxias", Value = "Galaxias" },
                    new SelectListItem { Text = "Estrellas", Value = "Estrellas"  },
                    new SelectListItem { Text = "Nebulosas", Value = "Nebulosas"  },
                    new SelectListItem { Text = "Planetarias", Value = "Planetarias"  }
                    },
                    JsonRequestBehavior.AllowGet
                    );
                case "Astronomia":
                    return Json(
                    new List<SelectListItem>() {
                    new SelectListItem { Text = "-Topico-", Value = null},
                    new SelectListItem { Text = "Astronomia Observacional", Value = "Astronomia Observacional" },
                    new SelectListItem { Text = "Astronomia Teorica", Value = "Astronomia Teorica"  },
                    new SelectListItem { Text = "Mecanica Celeste", Value = "Mecanica Celeste"  },
                    new SelectListItem { Text = "Astrofisica", Value = "Astrofisica"  },
                    new SelectListItem { Text = "Astroquimica", Value = "Astroquimica"  },
                    new SelectListItem { Text = "Astrobiologia", Value = "Astrobiologia"  }
                    },
                    JsonRequestBehavior.AllowGet
                    );
                case "General":
                    return Json(
                    new List<SelectListItem>() {
                    new SelectListItem { Text = "-Topico-", Value = null},
                    new SelectListItem { Text = "Astrofotografia", Value = "Astrofotografia" },
                    new SelectListItem { Text = "Instrumentos", Value = "Instrumentos"  },
                    new SelectListItem { Text = "Pregunta Sencilla", Value = "Pregunta Sencilla"  }
                    },
                    JsonRequestBehavior.AllowGet
                    );

            }
            return null;
        }

       public ActionResult verNoticia(string stringId)
        {
            NoticiasHandler accesoDatos = new NoticiasHandler();
            ViewBag.noticia = accesoDatos.ObtenerNoticia(stringId);
            ViewBag.topicos = accesoDatos.ObtenerTopicos(stringId);
            return View();
        }

        [HttpGet]
        public ActionResult ObtenerImagen(string numNoticia)
        {
            NoticiasHandler noticiaHandler = new NoticiasHandler();
            ViewBag.id = numNoticia;
            var tupla = noticiaHandler.ObtenerFoto(numNoticia);
            return File(tupla.Item1, tupla.Item2);
        }

        
    }
}