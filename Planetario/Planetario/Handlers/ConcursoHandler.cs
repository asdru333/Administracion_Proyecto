using System;
using System.Collections.Generic;
using System.Data;
using Planetario.Models;
using System.Web;
using Planetario.Interfaces;

namespace Planetario.Handlers
{
    public class ConsursoHandler : BaseDatosHandler, ConcursoInterfaz
    {
        private List<ConsursoModel> ConvertirTablaALista(DataTable tabla)
        {
            List<ConsursoModel> concursos = new List<ConsursoModel>();
            foreach (DataRow columna in tabla.Rows)
            {
                concursos.Add(
                    new ConsursoModel
                    {
                        NombreConcurso = Convert.ToString(columna["nombreConcursoPK"]),
                        Fecha = Convert.ToString(columna["fechaDeCreacion"]),
                        Inscripcion = Convert.ToBoolean(columna["inscripcion"]),
                        Abierto = Convert.ToBoolean(columna["abierto"]),
                        Premio = Convert.ToString(columna["premio"]),
                        PropuestoPor = Convert.ToString(columna["correoFuncionarioFK"]),
                        Ganador = Convert.ToString(columna["ganadorFK"])
                    });
            }
            return concursos;
        }

        private List<ConsursoModel> ObtenerConcursos (string consulta)
        {
            DataTable tabla = LeerBaseDeDatos(consulta);
            List<ConsursoModel> lista = ConvertirTablaALista(tabla);
            return lista;
        }

        public List<ConsursoModel> ObtenerTodosLosConcursos()
        {
            string consulta = "SELECT * FROM Concurso";
            return (ObtenerConcursos(consulta));
        }

        public List<ConsursoModel> ObtenerConcursosInscribibles(int incripcion)
        { 
            string consulta = "SELECT * FROM Concurso WHERE inscripcion = " + incripcion + ";";
            return (ObtenerConcursos(consulta));
        }

        public List<ConsursoModel> ObtenerConcursosAbiertos(int abierto)
        {
            string consulta = "SELECT * FROM Concurso WHERE inscripcion = " + abierto + ";";
            return (ObtenerConcursos(consulta));
        }

        public bool InsertarConcurso(ConsursoModel concurso)
        {
            string consulta =
                "INSERT INTO Concurso (nombreConcursoPK, fechaDeCreacion, inscripcion, abierto, premio, correoFuncionarioFF, ganadorFK) " +
                "VALUES ( @nombreConcursoPK, @fecha, @inscripcion, @abierto, @premio, propuestoPor, @ganador )";

            if (concurso.Ganador == null) { concurso.Ganador = "No hay todavía"; }

            Dictionary<string, object> valoresParametros = new Dictionary<string, object> {
                {"@nombreActividadPK", concurso.NombreConcurso },
                {"@fecha", concurso.Fecha },
                {"@inscripcion", concurso.Inscripcion },
                {"@abierto", concurso.Abierto },
                {"@premio", concurso.Premio },
                {"@propuestoPor", concurso.PropuestoPor },
                {"@ganador", concurso.Ganador}
            };
            return (InsertarEnBaseDatos(consulta, valoresParametros));
        }

        public ConsursoModel ObtenerConcurso(string nombre)
        {
            string consulta = "Select * FROM Concurso WHERE nombreConcursoPK = '" + nombre + "';";
            return (ObtenerConcursos(consulta)[0]);
        }

        public List<string> ObtenerParticipantes(int nombreConcurso)
        {
            string consulta = "SELECT P.correoPersonaPK FROM InscritosConcurso Ic JOIN Persona P ON Ic.ganadorFK = P.correoPersonaPK WHERE Ic.nombreConcursoFK = '" + nombreConcurso  + "';";
            DataTable tablaResultados = LeerBaseDeDatos(consulta);
            List<string> participantes = new List<string>();
            foreach (DataRow fila in tablaResultados.Rows)
            {
                participantes.Add(Convert.ToString(fila["correoPersonaPK"]));
            }
            return participantes;
        }
    }
}