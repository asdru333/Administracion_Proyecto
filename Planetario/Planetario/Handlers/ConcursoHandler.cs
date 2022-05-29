using System;
using System.Collections.Generic;
using System.Data;
using Planetario.Models;
using System.Web;
using Planetario.Interfaces;

namespace Planetario.Handlers
{
    public class ConcursoHandler : BaseDatosHandler, ConcursoInterfaz
    {
        private List<ConcursoModel> ConvertirTablaALista(DataTable tabla)
        {
            List<ConcursoModel> concursos = new List<ConcursoModel>();
            foreach (DataRow columna in tabla.Rows)
            {
                concursos.Add(
                    new ConcursoModel
                    {
                        NombreConcurso = Convert.ToString(columna["nombreConcursoPK"]),
                        Tema = Convert.ToString(columna["tema"]),
                        Descripcion = Convert.ToString(columna["descripcion"]),
                        Fecha = Convert.ToString(columna["fecha"]),
                        Inscripcion = Convert.ToBoolean(columna["inscripcion"]),
                        Abierto = Convert.ToBoolean(columna["abierto"]),
                        Premio = Convert.ToString(columna["premio"]),
                        PropuestoPor = Convert.ToString(columna["correoFuncionarioFK"]),
                        Ganador = Convert.ToString(columna["ganadorFK"])
                    });
            }
            return concursos;
        }

        private List<ConcursoModel> ObtenerConcursos (string consulta)
        {
            DataTable tabla = LeerBaseDeDatos(consulta);
            List<ConcursoModel> lista = ConvertirTablaALista(tabla);
            return lista;
        }

        public List<ConcursoModel> ObtenerTodosLosConcursos()
        {
            string consulta = "SELECT * FROM Concurso";
            return (ObtenerConcursos(consulta));
        }

        public List<ConcursoModel> ObtenerConcursosInscribibles(int incripcion)
        { 
            string consulta = "SELECT * FROM Concurso WHERE inscripcion = " + incripcion + ";";
            return (ObtenerConcursos(consulta));
        }

        public List<ConcursoModel> ObtenerConcursosAbiertos(int abierto)
        {
            string consulta = "SELECT * FROM Concurso WHERE inscripcion = " + abierto + ";";
            return (ObtenerConcursos(consulta));
        }

        public ConcursoModel ObtenerConcurso(string nombre)
        {
            string consulta = "Select * FROM Concurso WHERE nombreConcursoPK = '" + nombre + "';";
            return (ObtenerConcursos(consulta)[0]);
        }

        public bool InsertarConcurso(ConcursoModel concurso)
        {
            string consulta =
                "INSERT INTO Concurso (nombreConcursoPK, tema, descripcion, fecha, inscripcion, abierto, premio, correoFuncionarioFK, ganadorFK) " +
                "VALUES ( @nombreConcursoPK, @tema, @descripcion, @fecha, 1, 1, @premio, @propuestoPor, NULL )";

            Dictionary<string, object> valoresParametros = new Dictionary<string, object> {
                {"@nombreConcursoPK", concurso.NombreConcurso },
                {"@tema", concurso.Tema },
                {"@descripcion", concurso.Descripcion },
                {"@fecha", concurso.Fecha },
                {"@premio", concurso.Premio },
                {"@propuestoPor", HttpContext.Current.User.Identity.Name }
            };
            return (InsertarEnBaseDatos(consulta, valoresParametros));
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

        public bool Inscribirse(string participante, string concurso)
        {
            string consulta = "INSERT INTO InscritosConcurso (correoPersonaFK, nombreConcursoFK) VALUES ( @correoPersonaFK, @nombreConcursoFK)";
            Dictionary<string, object> valoresParametros = new Dictionary<string, object> {
                {"@correoPersonaFK", participante },
                {"@nombreConcursoFK", concurso }
            };
            return (InsertarEnBaseDatos(consulta, valoresParametros));
        }
    }
}