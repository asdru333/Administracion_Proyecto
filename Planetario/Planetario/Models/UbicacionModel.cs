using System.ComponentModel.DataAnnotations;

namespace Planetario.Models
{
    public class UbicacionModel : InscripcionModel
    {
        [Display(Name = "País")]
        [Required(ErrorMessage = "Es necesario que se ingrese un pais.")]
        public string Pais { get; set; }

        [Display(Name = "Nombre completo del recibidor")]
        [Required(ErrorMessage = "Es necesario que se ingrese un nombre.")]
        public string Nombre { get; set; }

        [Display(Name = "Dirección de envío")]
        [Required(ErrorMessage = "Es necesario que se ingrese una direccion.")]
        public string Direccion { get; set; }

        [Display(Name = "Ciudad")]
        [Required(ErrorMessage = "Es necesario que se ingrese una ciudad.")]
        public string Ciudad { get; set; }

        [Display(Name = "Estado / Provincia / Región")]
        public string Estado { get; set; }

        [Display(Name = "Código Postal")]
        [Required(ErrorMessage = "Es necesario que se ingrese un código postal.")]
        public string Codigo { get; set; }

        [Display(Name = "Número de teléfono")]
        [Required(ErrorMessage = "Es necesario que se ingrese un número.")]
        public string Telefono { get; set; }
    }
}