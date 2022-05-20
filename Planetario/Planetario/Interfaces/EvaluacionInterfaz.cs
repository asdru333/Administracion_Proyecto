using Planetario.Models;
using System.Collections.Generic;

namespace Planetario.Interfaces
{
    public interface EvaluacionInterfaz
    {
        CuestionarioEvaluacionRecibirModel ObtenerCuestionarioRecibir(string nombreCuestionario);

        bool InsertarRespuestas(CuestionarioEvaluacionRecibirModel evaluacion);

        bool InsertarComentario(CuestionarioEvaluacionRecibirModel evaluacion);

        bool InsertarFuncionalidadesEvaluadas(CuestionarioEvaluacionRecibirModel evaluacion);

        List<int> ObtenerCantidadRespuestasPorPreguntaYFecha(int preguntaID, string fechaInicio, string fechaFinal);

        int ObtenerCantidadPersonas(string nombreCuestionario);

        List<int> ObtenerCantidadRespuestasPorPregunta(int preguntaID);

        CuestionarioEvaluacionMostrarModel ObtenerCuestionarioMostrar(string nombreCuestionario);

        int ObtenerCantidadPersonasPorFecha(string nombreCuestionario, string fechaInicio, string fechaFinal);

        string ObtenerRelaciones(string nombreCuestionario, List<string> preguntas, List<string> opciones, string fechaInicio, string fechaFin);

        List<string> ObtenerComentariosDeCuestionario(string nombreCuestionario);

        List<string> ObtenerComentariosDeCuestionarioPorFecha(string nombreCuestionario, string fechaInicio, string fechaFinal);
    }
}