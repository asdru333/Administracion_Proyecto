using Planetario.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Data;

namespace Planetario.Handlers
{
    public class PreguntasFrecuentesHandler
    {
        private readonly BaseDatosHandler BaseDatos;
        private string Consulta;

        public PreguntasFrecuentesHandler()
        {
            BaseDatos = new BaseDatosHandler();
        }

        public List<PreguntasFrecuentesModel> ObtenerPreguntasFrecuentes()
        {
            List<PreguntasFrecuentesModel> preguntasFrecuentes = new List<PreguntasFrecuentesModel>();
            List<string> preguntasFrecuentesTopicos;
            Consulta = "SELECT DISTINCT * FROM dbo.PreguntasFrecuentes";
            DataTable tablaResultado = BaseDatos.LeerBaseDeDatos(Consulta);
            DataTable tablaResultadoTopicos;

            foreach (DataRow columna in tablaResultado.Rows)
            {
                Consulta = "SELECT DISTINCT * FROM dbo.PreguntasFrecuentesTopicos WHERE idPreguntaFK = " + Convert.ToInt32(columna["idPreguntaPK"]);
                tablaResultadoTopicos = BaseDatos.LeerBaseDeDatos(Consulta);
                preguntasFrecuentesTopicos = new List<string>();
                string topico1 = "NULL";
                string topico2 = "NULL";
                string topico3 = "NULL";

                foreach (DataRow columna2 in tablaResultadoTopicos.Rows)
                {
                    preguntasFrecuentesTopicos.Add(Convert.ToString(columna2["topicosPreguntasFrecuentes"]));
                }

                for(int i = 0; i < preguntasFrecuentesTopicos.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            topico1 = preguntasFrecuentesTopicos[i];
                            break;
                        case 1:
                            topico2 = preguntasFrecuentesTopicos[i];
                            break;
                        case 2:
                            topico3 = preguntasFrecuentesTopicos[i];
                            break;
                    }              
                }

                preguntasFrecuentes.Add(
                    new PreguntasFrecuentesModel
                    {
                        idPregunta = Convert.ToInt32(columna["idPreguntaPK"]),                      
                        categoriaPregunta = Convert.ToString(columna["categoriaPreguntasFrecuentes"]),
                        pregunta = Convert.ToString(columna["pregunta"]),
                        respuesta = Convert.ToString(columna["respuesta"]),
                        correoFuncionario = Convert.ToString(columna["correoFuncionarioFK"]),
                        topicoPregunta = topico1,
                        topicoPregunta2 = topico2,
                        topicoPregunta3 = topico3,               
                    });
            }

            return preguntasFrecuentes;
        }

        public List<String> ObtenerCategorias()
        {
            List<String> categorias = new List<String>();
            Consulta = "SELECT DISTINCT categoriaPreguntasFrecuentes FROM dbo.PreguntasFrecuentes";
            DataTable tablaResultado = BaseDatos.LeerBaseDeDatos(Consulta);

            foreach (DataRow columna in tablaResultado.Rows)
            {
                categorias.Add(Convert.ToString(columna["categoriaPreguntasFrecuentes"]));
            }

            return categorias;
        }

        public bool agregarNuevaPregunta(PreguntasFrecuentesModel nuevaPregunta)
        {     
            bool exito;
            Consulta =
            "INSERT INTO dbo.PreguntasFrecuentes(pregunta, respuesta, correoFuncionarioFK, categoriaPreguntasFrecuentes) VALUES(@pregunta, @respuesta, @correoFuncionario, @categoriaPregunta);" +
            "DECLARE @identity int = scope_identity();" +
            "INSERT INTO dbo.PreguntasFrecuentesTopicos(idPreguntaFK, topicosPreguntasFrecuentes) VALUES(@identity, @topicoPregunta);";
            Dictionary<string, object> valoresParametros = new Dictionary<string, object>
            {
                { "@topicoPregunta",    nuevaPregunta.topicoPregunta },
                { "@categoriaPregunta", nuevaPregunta.categoriaPregunta },
                { "@pregunta",          nuevaPregunta.pregunta },
                { "@respuesta",         nuevaPregunta.respuesta },
                { "@correoFuncionario", HttpContext.Current.User.Identity.Name} 

            };

            if(nuevaPregunta.topicoPregunta2 != "-Topico-")
            {
                valoresParametros.Add("@topicoPregunta2", nuevaPregunta.topicoPregunta2);
                Consulta += "INSERT INTO dbo.PreguntasFrecuentesTopicos(idPreguntaFK, topicosPreguntasFrecuentes) VALUES(@identity, @topicoPregunta2);";
            }
            else
            {
                valoresParametros.Add("@topicoPregunta2", "NULL");
            }

            if (nuevaPregunta.topicoPregunta3 != "-Topico-")
            {
                valoresParametros.Add("@topicoPregunta3", nuevaPregunta.topicoPregunta3);
                Consulta += "INSERT INTO dbo.PreguntasFrecuentesTopicos(idPreguntaFK, topicosPreguntasFrecuentes) VALUES(@identity, @topicoPregunta3);";
            }
            else
            {
                valoresParametros.Add("@topicoPregunta3", "NULL");
            }

            exito = BaseDatos.InsertarEnBaseDatos(Consulta, valoresParametros);

            return exito;
        }
    }
}

    
