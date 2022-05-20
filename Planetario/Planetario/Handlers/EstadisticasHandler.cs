using System;
using System.Collections.Generic;
using System.Data;
using Planetario.Models;

namespace Planetario.Handlers
{
    public class EstadisticasHandler : BaseDatosHandler
    {
        public struct Filtro
        {
            public string columna;
            public List<string> opciones;
        }      
        
        public int obtenerCantidadDeParticipantes(string diaSemana, string publicoMeta, string nivelComplejidad, string categoria, string topico)
        {
            int cantidadTotal; 

            string consulta = crearStringDeConsultaCantidad(diaSemana, publicoMeta, nivelComplejidad, categoria, topico);

            DataTable tablaResultado = LeerBaseDeDatos(consulta);
            DataRow columna = tablaResultado.Rows[0];

            cantidadTotal = Convert.ToInt32(columna["Participantes"]);

            return cantidadTotal;
        }
        

        public List<string> obtenerListaIdiomas()
        {
            string consulta = "SELECT idioma as 'nombreIdioma' " +
                              "FROM FuncionarioIdioma " +
                              "GROUP BY idioma " +
                              "ORDER BY idioma ";

            DataTable tablaResultados = LeerBaseDeDatos(consulta);
            List<string> idiomas = new List<string>();

            foreach (DataRow fila in tablaResultados.Rows)
            {
                idiomas.Add(Convert.ToString(fila["nombreIdioma"]));
            }

            return idiomas;
        }

        public int obtenerNumIdiomas(string idioma)
        {
            string consulta = "SELECT COUNT(*) as 'numFuncionarios' " +
                              "FROM FuncionarioIdioma " +
                              "WHERE idioma = '" + idioma + "'";

            int numIdiomas;

            DataTable tablaResultados = LeerBaseDeDatos(consulta);
            DataRow columna = tablaResultados.Rows[0];

            numIdiomas = Convert.ToInt32(columna["numFuncionarios"]);

            return numIdiomas;
        }

        public List<EstadisticasModel> obtenerFuncionarios(List<string> idiomas)
        {
            List<EstadisticasModel> funcionarios = new List<EstadisticasModel>();
            string consulta = "SELECT DISTINCT F.nombre AS 'Nombre', " +
                              "F.apellido1 AS 'Apellido1', " +
                              "F.correoPK AS 'Correo', " +
                              "F.areaExpertis AS 'Expertis', " +
                              "STUFF( (SELECT ', ' + FI.idioma FROM dbo.FuncionarioIdioma FI WHERE FI.correoFuncionarioFK = correoPK FOR XML PATH('')), 1, 1, '') AS 'Idiomas' " +
                              "FROM Funcionario F JOIN FuncionarioIdioma I  " +
                              "ON F.correoPK = I.correoFuncionarioFK " +
                              "WHERE 1 = 1";

            foreach(var idioma in idiomas)
            {
                consulta += crearStringDeConsultaIdiomas(idioma);
            }


            DataTable tablaResultados = LeerBaseDeDatos(consulta);

            foreach (DataRow columna in tablaResultados.Rows)
            {
                funcionarios.Add(
                new EstadisticasModel
                {
                    Nombre = Convert.ToString(columna["Nombre"]),
                    Apellido = Convert.ToString(columna["Apellido1"]),
                    Correo = Convert.ToString(columna["Correo"]),
                    Expertis = Convert.ToString(columna["Expertis"]),
                    Idiomas = Convert.ToString(columna["Idiomas"])
                });
            }

            return funcionarios;
        }


        public string crearStringDeConsultaCantidad(string diaSemana, string publicoMeta, string nivelComplejidad, string categoria, string topico)
        {
            string consulta = "SELECT COUNT(*) as 'Participantes' " +
                              "FROM Factura F JOIN Actividad A " +
                              "ON A.nombreActividadPK = F.nombreActividadFK " +
                              "JOIN ActividadTopicos T " +
                              "ON A.nombreActividadPK = T.nombreActividadFK ";

            if (diaSemana != "")
            {
                consulta += " WHERE A.diaSemana = '" + diaSemana + "' ";
            }

            if (publicoMeta != "")
            {
                consulta += " AND A.publicoDirigidoActividad = '" + publicoMeta + "' ";
            }

            if (nivelComplejidad != "")
            {
                consulta += " AND A.complejidad = '" + nivelComplejidad + "' ";
            }

            if (categoria != "")
            {
                consulta += " AND A.categoriaActividad = '" + categoria + "' ";
            }

            if (topico != "")
            {
                consulta += " AND T.topicosActividad = '" + topico + "' ";
            }

            return consulta;
        }

        public string crearStringDeConsultaIdiomas(string idioma) 
        {
            string consulta = "";

            if (idioma != "") 
            {
                consulta += "AND '" + idioma + "' IN (SELECT FI.idioma FROM FuncionarioIdioma FI WHERE FI.correoFuncionarioFK = correoPK) ";
            }

            return consulta;
        }

    }
}