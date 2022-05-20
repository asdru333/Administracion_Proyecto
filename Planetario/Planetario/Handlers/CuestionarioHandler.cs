using System;
using System.Collections.Generic;
using System.Data;
using Planetario.Models;

namespace Planetario.Handlers
{
    public class CuestionarioHandler : BaseDatosHandler
    {
        public List<CuestionarioModel> obtenerCuestinariosSimple()
        {
            List<CuestionarioModel> cuestionarios = new List<CuestionarioModel>();
            string consulta = "Select * FROM Cuestionario";
            DataTable tablaResultado = LeerBaseDeDatos(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                cuestionarios.Add(
                new CuestionarioModel
                {
                    NombreCuestionario= Convert.ToString(columna["nombreCuestionarioPK"]),
                    EmbedHTML = Convert.ToString(columna["embedHTML"]),
                    CorreoResponsable = Convert.ToString(columna["correoFuncionarioFK"]),
                    Dificultad = Convert.ToString(columna["dificultad"]),
                });
            }
            return cuestionarios;
        }

        public List<CuestionarioModel> obtenerCuestinarioPorDificultad(string dificultad)
        {
            List<CuestionarioModel> cuestionarios = new List<CuestionarioModel>();
            string consulta = "Select * FROM Cuestionario";
            if (dificultad != "")
                consulta += " WHERE dificultad = '" + dificultad + "';";
            DataTable tablaResultado = LeerBaseDeDatos(consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                cuestionarios.Add(
                new CuestionarioModel
                {
                    NombreCuestionario = Convert.ToString(columna["nombreCuestionarioPK"]),
                    EmbedHTML = Convert.ToString(columna["embedHTML"]),
                    CorreoResponsable = Convert.ToString(columna["correoFuncionarioFK"]),
                    Dificultad = Convert.ToString(columna["dificultad"]),
                });
            }
            return cuestionarios;
        }

        public CuestionarioModel buscarCuestionario(string nombre)
        {
            CuestionarioModel cuestionario = null;
            string consulta = "Select * FROM Cuestionario WHERE nombreCuestionarioPK = '" + nombre + "';";
            DataTable tablaResultado = LeerBaseDeDatos(consulta);
            if (tablaResultado.Rows[0] != null)
            {
                cuestionario = new CuestionarioModel
                {
                    NombreCuestionario = Convert.ToString(tablaResultado.Rows[0]["nombreCuestionarioPK"]),
                    Dificultad = Convert.ToString(tablaResultado.Rows[0]["dificultad"]),
                    EmbedHTML = Convert.ToString(tablaResultado.Rows[0]["EmbedHTML"]),
                    CorreoResponsable = Convert.ToString(tablaResultado.Rows[0]["correoFuncionarioFK"]),
                };
            }
            return cuestionario;
        }

        public bool agregarCuestionario(CuestionarioModel cuestionario)
        {
            bool exito;
            string Consulta = "INSERT INTO Cuestionario (nombreCuestionarioPK, embedHTML, dificultad, correoFuncionarioFK) "
                                + "VALUES (@nombreCuestionario, @HTML, @dificultad, @correoResponsable) ";

            Dictionary<string, object> valoresParametros = new Dictionary<string, object> {
                {"@nombreCuestionario", cuestionario.NombreCuestionario },
                {"@HTML", cuestionario.EmbedHTML },
                {"@dificultad", cuestionario.Dificultad },
                {"@correoResponsable", cuestionario.CorreoResponsable}
            };
            exito = InsertarEnBaseDatos(Consulta, valoresParametros);
            return exito;
        }
    }
}

