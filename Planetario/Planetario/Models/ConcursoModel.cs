using System.ComponentModel.DataAnnotations;

namespace Planetario.Models
{
    public class ConsursoModel
    {
        [Required(ErrorMessage = "Es necesario que le indique el nombre que va a tener el concurso.")]
        [Display(Name = "Nombre del consurso")]
        [MaxLength(50, ErrorMessage = "Se tiene un máximo de 50 cáracteres")]
        public string NombreConcurso { get; set; }

        [Display(Name = "Fecha de creación")]
        public string Fecha { get; set; }

        [Display(Name = "Se puede inscribir")]
        public bool Inscripcion { get; set; }

        [Display(Name = "Está abierto")]
        public bool Abierto { get; set; }

        [Required(ErrorMessage = "Es necesario que indique el premio que se va a ofrecer")]
        [Display(Name = "Premio")]
        [MaxLength(100, ErrorMessage = "Se tiene un máximo de 100 cáracteres")]
        public string Premio { get; set; }

        [Display(Name = "Correo")]
        public string PropuestoPor { get; set; }

        [Display(Name = "Ganador")]
        public string Ganador { get; set; }
    }
}