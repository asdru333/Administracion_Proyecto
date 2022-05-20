using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Planetario.Models
{
    public class MaterialEducativoModel
    { 
        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "Es necesario indicar un titulo para el material")]
        public string Titulo { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "Es necesario indicar una categoria para el material")]
        public string Categoria { get; set; }

        [Display(Name = "Imagen de vista previa")]
        public HttpPostedFileBase ImagenVistaPrevia { get; set; }

        public string TipoArchivoVistaPrevia { get; set; }

        [Display(Name = "Archivo")]
        [Required(ErrorMessage = "Es necesario subir un archivo")]
        public HttpPostedFileBase MaterialArchivo { get; set; }

        public string MaterialTipoArchivo { get; set; }
        public string CorreoResponsable { get; set; }

        [Display(Name = "Nombre del responsable")]
        public string NombreResponsable { get; set; }

        [Display(Name = "Publico dirigido")]
        public string PublicoDirigido { get; set; }

        /** Hay que implementar seleccion de topicos en la view y handler
        [Display(Name = "Topicos")]
        [Required(ErrorMessage = "Es necesarios que escoja al menos un tópico")]
        public IList<string> Topicos { get; set; }
        */

        public bool HayVistaPrevia()
        {
            return ImagenVistaPrevia != null;
        }

    }
}