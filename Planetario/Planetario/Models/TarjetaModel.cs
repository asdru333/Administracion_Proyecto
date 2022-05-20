using System.ComponentModel.DataAnnotations;

namespace Planetario.Models
{
    public class TarjetaModel
    {
        [Display(Name = "Número de la tarjeta")]
        [Required(ErrorMessage = "Es necesario que ingrese un número")]
        [MaxLength(16, ErrorMessage = "Debe ingresar 16 dígitos")]
        [MinLength(16, ErrorMessage = "Debe ingresar 16 dígitos")]
        public string NumeroTarjeta { get; set; }

        [Display(Name = "Nombre en la tarjeta")]
        [Required(ErrorMessage = "Es necesario que ingrese un nombre")]
        public string NombreTarjeta { get; set; }

        [Display(Name = "Expiración (mm/aa)")]
        [Required(ErrorMessage = "Es necesario que ingrese una fecha")]
        public string FechaExpiracion { get; set; }

        [Display(Name = "CVV")]
        [Required(ErrorMessage = "Es necesario que ingrese un CVV")]
        [MaxLength(4, ErrorMessage = "Máximo de 4 cáracteres")]
        [MinLength(3, ErrorMessage = "Minimo de 4 cáracteres")]
        public string CVV { get; set; }

        [Display(Name = "Provincia o estado")]
        [Required(ErrorMessage = "Es necesario que ingrese una provincia")]
        public string provincia { get; set; }

        [Display(Name = "Ciudad")]
        [Required(ErrorMessage = "Es necesario que ingrese una ciudad")]
        public string ciudad { get; set; }

        [Display(Name = "Detalles de dirección")]
        [Required(ErrorMessage = "Es necesario que ingrese detalles de direccion")]
        public string detallesDireccion { get; set; }

        [Display(Name = "Código Postal")]
        [Required(ErrorMessage = "Es necesario que ingrese un codigo postal")]
        public string codigoPostal { get; set; }
    }
}