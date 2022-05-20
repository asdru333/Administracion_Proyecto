using System.ComponentModel.DataAnnotations;

namespace Planetario.Models
{
    public class CuestionarioModel
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Es necesario indicar el nombre del cuestionario")]
        public string NombreCuestionario { get; set; }

        [Display(Name = "EmbedHTML")]
        [Required(ErrorMessage = "Es necesario que ingrese el embed HTML")]
        public string EmbedHTML { get; set; }

        [Display(Name = "Correo del responsable")]
        [Required(ErrorMessage = "Es necesario indicar el correo de un colaborador")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string CorreoResponsable { get; set; }

        [Display(Name = "Dificultad")]
        [Required(ErrorMessage = "Es necesario que ingrese la dificultad del cuestionario")]
        public string Dificultad { get; set; }
    }
}