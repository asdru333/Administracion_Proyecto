using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Planetario.Models
{
    public class NoticiaModel
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Es necesario que indique el título de la noticia.")]
        [Display(Name = "Título ")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Es necesario que indique el cuerpo de la noticia.")]
        [Display(Name = "Cuerpo ")]
        public string Cuerpo { get; set; }

        [Required(ErrorMessage = "Es necesario que indique la fecha de la noticia.")]
        [Display(Name = "Fecha ")]
        public string Fecha { get; set; }

        [Display(Name = "Autor ")]
        public string CorreoAutor { get; set; }

        [Display(Name = "Categoría: ")]
        [Required(ErrorMessage = "Es necesario que ingrese la categoría de la noticia")]
        [MaxLength(100, ErrorMessage = "Se tiene un máximo de 100 cáracteres")]
        public string CategoriaNoticia { get; set; }

        [Display(Name = "Imagen1")]
        public HttpPostedFileBase Imagen1 { get; set; }

        public string TipoImagen1 { get; set; }

        [Display(Name = "Imagen2")]
        public HttpPostedFileBase Imagen2 { get; set; }

        public string TipoImagen2 { get; set; }
    }
}