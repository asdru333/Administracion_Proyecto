using System.ComponentModel.DataAnnotations;

namespace Planetario.Models
{
    public class PreguntasFrecuentesModel
    {
        public int idPregunta { get; set; }

        public string correoFuncionario { get; set; }

        [Display(Name = "Categoría: ")]
        [Required(ErrorMessage = "Es necesario que ingrese la categoría de la pregunta")]
        [MaxLength(100, ErrorMessage = "Se tiene un máximo de 100 cáracteres")]
        public string categoriaPregunta { get; set; }

        [Display(Name = "Tópico 1: ")]
        [Required(ErrorMessage = "Es necesario que ingrese el tópico de la pregunta")]
        [MaxLength(100, ErrorMessage = "Se tiene un máximo de 100 cáracteres")]
        public string topicoPregunta { get; set; }

        [Display(Name = "Tópico 2: ")]
        [Required(ErrorMessage = "Es necesario que ingrese el tópico de la pregunta")]
        [MaxLength(100, ErrorMessage = "Se tiene un máximo de 100 cáracteres")]
        public string topicoPregunta2 { get; set; }

        [Display(Name = "Tópico 3: ")]
        [Required(ErrorMessage = "Es necesario que ingrese el tópico de la pregunta")]
        [MaxLength(100, ErrorMessage = "Se tiene un máximo de 100 cáracteres")]
        public string topicoPregunta3 { get; set; }

        [Display(Name = "Ingrese la pregunta:")]
        [Required(ErrorMessage = "Es necesario que ingrese la pregunta")]
        public string pregunta { get; set; }

        [Display(Name = "Ingrese la respuesta:")]
        [Required(ErrorMessage = "Es necesario que ingrese la respuesta")]
        public string respuesta { get; set; }
    }
}
