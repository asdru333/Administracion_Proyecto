using System.ComponentModel.DataAnnotations;

namespace Planetario.Models
{
    public class PersonaModel
    {
        [Display(Name = "Correo Electrónico")]
        [Required(ErrorMessage = "Es necesario que ingrese su correo electrónico")]
        [EmailAddress(ErrorMessage = "Formato incorrecto")]
        public string correo{ get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Es necesario que ingrese su nombre")]
        public string nombre { get; set; }

        [Display(Name = "Primer apellido")]
        [Required(ErrorMessage = "Es necesario que ingrese su primer apellido")]
        public string apellido1 { get; set; }

        [Display(Name = "Segundo apellido")]
        public string apellido2 { get; set; }

        [Display(Name = "País")]
        [Required(ErrorMessage = "Es necesario que ingrese su país")]
        public string pais { get; set; }

        [Display(Name = "Género")]
        [Required(ErrorMessage = "Es necesario que ingrese su género")]
        public string genero { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [Required(ErrorMessage = "Es necesario que ingrese una fecha de nacimiento")]
        public string fechaNacimiento { get; set; }

        [Display(Name = "Nivel Educativo")]
        [Required(ErrorMessage = "Es necesario que ingrese su nivel educativo")]
        public string nivelEducativo { get; set; }

        public string membresia { get; set; }

        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        [StringLength(150, MinimumLength = 6)]
        [Required(ErrorMessage = "Es necesario que ingrese su contraseña")]
        public string contrasena { get; set; }
    }
}