using System.Collections.Generic;

namespace Planetario.Models
{
    public class CuestionarioEvaluacionMostrarModel: CuestionarioEvaluacionModel
    {
        public List<List<int>> MatrizRespuestas { get; set; }
    }
}