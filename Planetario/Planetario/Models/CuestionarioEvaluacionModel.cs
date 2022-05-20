using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Planetario.Models
{
    public class CuestionarioEvaluacionModel
    {
        [Display(Name = "Nombre del cuestionario")]
        [Required(ErrorMessage = "Es necesario indicar el nombre del cuestionario.")]
        public string NombreCuestionario { get; set; }

        [Display(Name = "Categoria del cuestionario")]
        [Required(ErrorMessage = "Es necesario que ingrese la categoria del cuestionario.")]
        public string Categoria { get; set; }

        public List<string> Comentario { get; set; }
    }
}