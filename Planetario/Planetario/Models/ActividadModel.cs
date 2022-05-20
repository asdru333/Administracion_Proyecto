using System.ComponentModel.DataAnnotations;

namespace Planetario.Models
{
    public class ActividadModel
    {
        [Required(ErrorMessage = "Es necesario que le indique el nombre que va a tener la actividad.")]
        [Display(Name = "Nombre")]
        [MaxLength(100, ErrorMessage = "Se tiene un máximo de 100 cáracteres")]
        public string NombreActividad { get; set; }

        [Required(ErrorMessage = "Es necesario que indique la descripcion de la actividad.")]
        [Display(Name = "Descripcion")]
        [MaxLength(100, ErrorMessage = "Se tiene un máximo de 100 cáracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese aproximadamente cuantos minutos durara la actividad.")]
        [Display(Name = "Duración en minutos")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar números")]
        public int Duracion { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese que tan compleja es la actividad.")]
        [Display(Name = "Complejidad")]
        public string Complejidad { get; set; }

        [Required(ErrorMessage = "Es necesario que ingrese aproximadamente cuantos colones cuesta la actividad.")]
        [Display(Name = "Precio")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Debe ingresar números")]
        public double PrecioAproximado { get; set; }

        [Required(ErrorMessage = "Es necesario que indique la categoria de la actividad.")]
        [Display(Name = "Categoria")]
        
        public string Categoria { get; set; }
        
        [Display(Name = "Dia de la semana")]
        public string DiaSemana { get; set; }

        [Required(ErrorMessage = "Es necesario que indique la fecha")]
        [Display(Name = "Fecha")]
        public string Fecha { get; set; }

        [Display(Name = "Correo")]
        public string PropuestoPor { get; set; }

        [Display(Name = "Aprobado")]
        public bool Aprobado { get; set; }

        [Required(ErrorMessage = "Es necesario que indique el publico al que va dirigido la actividad.")]
        [Display(Name = "Publico dirigido")]
        public string PublicoDirigido { get; set; }

        [Display(Name = "Correo Aprobado")]
        public string AprobadoPor { get; set; }

        [Required(ErrorMessage = "Es necesario que indique el tipo de la actividad.")]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }

        [Display(Name = "Nombre del canal de Twitch (Ejm: monstercat)")]
        public string Link { get; set; }
    }
}