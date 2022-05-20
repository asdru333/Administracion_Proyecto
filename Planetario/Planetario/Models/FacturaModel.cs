using System.ComponentModel.DataAnnotations;

namespace Planetario.Models
{
    public class FacturaModel
    {
        public int id {get;set;}

        public string fecha { get; set; }

        public double pago { get; set; }

        public string correoCliente { get; set; }

        public string actividad { get; set; }

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