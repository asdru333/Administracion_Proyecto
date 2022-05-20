using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Diagnostics;

namespace Planetario.Handlers
{
    public class BaseDatosHandler
    {
        private SqlConnection conexion;
        private readonly string rutaConexion;

        public BaseDatosHandler()
        {
            rutaConexion = ConfigurationManager.ConnectionStrings["ConexionBaseAzure"].ToString();
            conexion = new SqlConnection(rutaConexion);
        }

        public DataTable LeerBaseDeDatos(string consulta)
        {
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();

            conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            conexion.Close();
            return consultaFormatoTabla;
        }

        public bool InsertarEnBaseDatos(string consulta, Dictionary<string, object> valoresParametros)
        {
            bool exito;
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            Debug.WriteLine(consulta);

            if(valoresParametros != null)
            { 
                foreach (KeyValuePair<string, object> parejaValores in valoresParametros)
                {
                    comandoParaConsulta.Parameters.AddWithValue(parejaValores.Key, parejaValores.Value);
                    Debug.WriteLine(parejaValores.Key + "\t\t" + parejaValores.Value);
                }
            }

            conexion.Open();
            try
            {
                comandoParaConsulta.ExecuteNonQuery();
                exito = true;
                Debug.WriteLine("exito");
            }
            catch(System.Exception ex)
            {
                Debug.WriteLine("error");
                System.Console.WriteLine(ex.Message);
                Debug.WriteLine(ex.Message);
                exito = false;
            }
            conexion.Close();
            return exito;
        }

        public bool EliminarEnBaseDatos(string consulta, Dictionary<string, object> valoresParametros)
        {
            bool exito;
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);

            if(valoresParametros != null)
                foreach (KeyValuePair<string, object> parejaValores in valoresParametros)
                {
                    comandoParaConsulta.Parameters.AddWithValue(parejaValores.Key, parejaValores.Value);
                }

            conexion.Open();
            
            try
            {
                exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            }
            catch(System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                exito = false;
            }

            conexion.Close();
            return exito;
        }

        public bool ActualizarEnBaseDatos(string consulta, Dictionary<string, object> valoresParametros)
        {
            bool exito;
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);

            if(valoresParametros != null)
            {
                foreach (KeyValuePair<string, object> parejaValores in valoresParametros)
                {
                    comandoParaConsulta.Parameters.AddWithValue(parejaValores.Key, parejaValores.Value);
                }
            }

            conexion.Open();
            exito = comandoParaConsulta.ExecuteNonQuery() >= 1;
            conexion.Close();

            return exito;
        }

        public bool InsertarEnBaseDatosConIsolación(string consulta, Dictionary<string, object> valoresParametros)
        {
            consulta.Insert(0, "SET TRANSACTION ISOLATION LEVEL REPEATABLE READ; BEGIN TRANSACTION ");
            consulta += " COMMIT TRANSACTION";
            return InsertarEnBaseDatos(consulta, valoresParametros);
        }

        public Tuple<byte[],string> ObtenerArchivo (string consulta, KeyValuePair<string,object> parametro, string columnaContenido, string columnaTipo)
        {
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.Parameters.AddWithValue(parametro.Key, parametro.Value);
            
            conexion.Open();
            SqlDataReader lectorDeDatos = comandoParaConsulta.ExecuteReader();
            lectorDeDatos.Read();
            byte[] bytes = (byte[])lectorDeDatos[columnaContenido];
            string tipo = tipo = lectorDeDatos[columnaTipo].ToString();
            conexion.Close();

            return new Tuple<byte[], string>(bytes, tipo);
        }
    }
}