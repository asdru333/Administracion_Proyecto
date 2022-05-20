using System.ComponentModel.DataAnnotations;

namespace Planetario.Models
{
    public class EventoModel
    {
        [Display(Name = "Título")]
        [Required(ErrorMessage = "Es necesario que ingrese un titulo para el evento")]
        public string Titulo { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "Es necesario que ingrese una fecha para el evento")]
        public string Fecha { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Es necesario que ingrese una descripción para el evento")]
        public string Descripcion { get; set; }

        [Display(Name = "Hora")]
        [Required(ErrorMessage = "Es necesario que ingrese una hora para el evento")]
        public string Hora { get; set; }

        [Display(Name = "Nombre del canal de Twitch (Ejm: monstercat)")]
        public string Link { get; set; }
    }
}