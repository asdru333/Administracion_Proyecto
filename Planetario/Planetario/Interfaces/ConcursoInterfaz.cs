﻿
using Planetario.Models;
using System.Collections.Generic;

namespace Planetario.Interfaces
{
    public interface ConcursoInterfaz
    {
        List<ConcursoModel> ObtenerTodosLosConcursos();

        List<ConcursoModel> ObtenerConcursosInscribibles(int incripcion);

        List<ConcursoModel> ObtenerConcursosAbiertos(int abierto);

        bool InsertarConcurso(ConcursoModel concurso);

        ConcursoModel ObtenerConcurso(string nombre);

        List<string> ObtenerParticipantes(int nombreConcurso);

        bool Inscribirse(string participante, string concurso);
    } 
}