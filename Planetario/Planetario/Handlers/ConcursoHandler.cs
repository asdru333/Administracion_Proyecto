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
            string consulta = "SELECT * FROM Concurso WHERE abierto = " + abierto + ";";
            return (ObtenerConcursos(consulta));
        }

        public List<ConcursoModel> ObtenerConcursosConGanadorDeclarado()
        {
            string consulta = "SELECT * FROM Concurso where ganadorFK is not NULL";
            return (ObtenerConcursos(consulta));
        }

        public ConcursoModel ObtenerConcurso(string nombre)
        {
            string consulta = "Select * FROM Concurso WHERE nombreConcursoPK = '" + nombre + "';";
            return (ObtenerConcursos(consulta)[0]);
        }
        
        public bool EliminarConcurso(string nombre)
        {
            string consulta = "DELETE FROM Concurso WHERE nombreConcursoPK = '" + nombre + "';" ;
            return EliminarEnBaseDatos(consulta, null);
        }

        public bool ActualizarConcurso(ConcursoModel concurso)
        {
            string consulta = "UPDATE Concurso SET tema = @tema , descripcion = @descripcion, fecha = @fecha, premio = @premio, " +
                              "inscripcion = 1, abierto = 1, correoFuncionarioFK = @propuestoPor, ganadorFK = NULL WHERE nombreConcursoPK = @nombreConcursoPK";

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

        public bool CerrarConcurso(string nombre)
        {
            string consulta = "UPDATE Concurso SET abierto = 0 WHERE nombreConcursoPK = '" + nombre + "';";
            return (InsertarEnBaseDatos(consulta, null));
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

        public IList<string> ObtenerParticipantes(string nombreConcurso)
        {
            string consulta = "SELECT P.correoPersonaPK FROM Persona P JOIN InscritosConcurso Ic ON Ic.correoPersonaFK = P.correoPersonaPK WHERE Ic.nombreConcursoFK = '" + nombreConcurso  + "';";
            DataTable tablaResultados = LeerBaseDeDatos(consulta);
            List<string> participantes = new List<string>();
            foreach (DataRow fila in tablaResultados.Rows)
            {
                participantes.Add(Convert.ToString(fila["correoPersonaPK"]));
            }
            return participantes;
        }

        public bool InsertarGanador(string concurso, string ganador)
        {
            string consulta = "UPDATE Concurso SET ganadorFK = '" + ganador + "' WHERE nombreConcursoPK = '" + concurso + "';";
            return (InsertarEnBaseDatos(consulta, null));
        }

        public bool Inscribirse(string concurso)
        {
            string consulta = "INSERT INTO InscritosConcurso (correoPersonaFK, nombreConcursoFK) VALUES ( @correoPersonaFK, @nombreConcursoFK)";
            Dictionary<string, object> valoresParametros = new Dictionary<string, object> {
                {"@correoPersonaFK", HttpContext.Current.User.Identity.Name },
                {"@nombreConcursoFK", concurso }
            };
            return (InsertarEnBaseDatos(consulta, valoresParametros));
        }

        public bool Desinscribirse(string concurso)
        {
            string consulta =  "delete from InscritosConcurso where correoPersonaFK = @correoPersonaFK and nombreConcursoFK = @nombreConcursoFK";
            Dictionary<string, object> valoresParametros = new Dictionary<string, object> {
                {"@correoPersonaFK", HttpContext.Current.User.Identity.Name },
                {"@nombreConcursoFK", concurso }
            };
            return (EliminarEnBaseDatos(consulta, valoresParametros));
        }

        public bool TieneInicioSesion()
        {
            if (HttpContext.Current.User.Identity.Name == "")
            {
                return false;
            }
            return true;
        }

        public bool EstaInscrito(string concurso)
        {
            if (HttpContext.Current.User.Identity.Name == "")
            {
                return false;
            }
            string consulta = "SELECT * FROM InscritosConcurso WHERE correoPersonaFK = '" + HttpContext.Current.User.Identity.Name +"' and nombreConcursoFK = '" + concurso + "';";
            DataTable tabla = LeerBaseDeDatos(consulta);
            return (tabla.Rows.Count > 0);
        }
    }
}