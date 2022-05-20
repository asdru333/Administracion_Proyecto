using System.Collections.Generic;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Planetario.Models
{
    public class FuncionarioModel: PersonaModel
    {
        

        [Display(Name = "Ingrese su descripción")]
        [Required(ErrorMessage = "Es necesario que ingrese su descripción")]
        [MaxLength(10000, ErrorMessage = "Se tiene un máximo de 200 cáracteres")]
        public string descripcion { get; set; }

        [Display(Name = "Fecha de incorporación")]
        public string fechaIncorporacion { get; set; }

        [Display(Name = "Area de expertís")]
        [Required(ErrorMessage = "Es necesario que ingrese el área en que es experto")]
        public string areaExpertis { get; set; }

        [Display(Name = "Idiomas")]
        //[Required(ErrorMessage = "Es necesario que ingrese al menos un idioma que domine")]
        public IList<string> idiomas { get; set; }

        [Display(Name = "Roles")]
        //[Required(ErrorMessage = "Es necesario que ingrese al menos un rol que va a ejercer")]
        public IList<string> roles { get; set; }

        [Display(Name = "Títulos")]
        //[Required(ErrorMessage = "Es necesario que ingrese al menos un título")]
        public IList<string> titulos { get; set; }

        [Display(Name = "Foto del funcionario")]
        public HttpPostedFileBase FotoArchivo { get; set; }

        public string TipoArchivoFoto { get; set; }

        [Display(Name = "Contrasena")]
        [DataType(DataType.Password)]
        [StringLength(150, MinimumLength = 6)]
        [Required(ErrorMessage = "Es necesario que ingrese su contrasena")]
        public string Contrasena { get; set; }

        public FuncionarioModel()
        {
            idiomas = new List<string>();
            roles = new List<string>();
            titulos = new List<string>();
        }
    }
}