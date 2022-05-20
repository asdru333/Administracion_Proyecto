using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Planetario.Models
{
    public class CuestionarioEvaluacionRecibirModel : CuestionarioEvaluacionModel
    {

        public List<string> Preguntas { get; set; }

        public List<string> Respuestas { get; set; }

        public CuestionarioEvaluacionRecibirModel()
        {
            Preguntas = new List<string>();
            Respuestas = new List<string>();
        }

        [Required(ErrorMessage = "Es necesario que escoje al menos una funcionalidad.")]
        [Display(Name = "Funcionalidades")]
        public string Funcionalidades { get; set; }
    }
}
