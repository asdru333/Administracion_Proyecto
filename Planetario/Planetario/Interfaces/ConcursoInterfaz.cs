
using Planetario.Models;
using System.Collections.Generic;

namespace Planetario.Interfaces
{
    public interface ConcursoInterfaz
    {
        List<ConsursoModel> ObtenerTodosLosConcursos();

        List<ConsursoModel> ObtenerConcursosInscribibles(int incripcion);

        List<ConsursoModel> ObtenerConcursosAbiertos(int abierto);

        bool InsertarConcurso(ConsursoModel concurso);

        ConsursoModel ObtenerConcurso(string nombre);

        List<string> ObtenerParticipantes(int nombreConcurso);

        bool Inscribirse(string participante, string concurso);
    } 
}