using System.ComponentModel.DataAnnotations;

namespace Planetario.Models
{
    public class ComprableModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Es necesario que se ingrese un nombre para el producto")]
        public string Nombre { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "Es necesario que ingrese un precio")]
        public double Precio { get; set; }

        [RegularExpression(@"^-?[0-9]\d{0,2}(\.\d{0,1})?$", ErrorMessage = "El valor ingresado debe ser un número")]
        public int CantidadDisponible { get; set; }

        public int CantidadCarrito { get; set; }
    }
}