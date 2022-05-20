using Planetario.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Planetario.Handlers
{
    public class ParticipanteHandler : BaseDatosHandler
    {
        private List<ClienteModel> ConvertirTablaALista(DataTable tabla)
        {
            List<ClienteModel> participantes = new List<ClienteModel>();
            foreach (DataRow columna in tabla.Rows)
            {
                participantes.Add(new ClienteModel
                {
                    correo = Convert.ToString(columna["correoParticipantePK"]),
                    nombre = Convert.ToString(columna["nombre"]),
                    apellido1 = Convert.ToString(columna["apellido1"]),
                    apellido2 = Convert.ToString(columna["apellido2"]),
                    genero = Convert.ToString(columna["genero"]),
                    pais = Convert.ToString(columna["pais"]),
                    fechaNacimiento = TranslateFecha(Convert.ToString(columna["fechaNacimiento"])),
                    nivelEducativo = Convert.ToString(columna["nivelEducativo"])
                });
            }
            return participantes;
        }

        private List<ClienteModel> ObtenerParticipantes(string consulta)
        {
            DataTable tabla = LeerBaseDeDatos(consulta);
            List<ClienteModel> lista = ConvertirTablaALista(tabla);
            return lista;
        }

        public ClienteModel ObtenerParticipante(string correo)
        {
            string consulta = "SELECT * FROM Participante WHERE correoParticipantePK = '" + correo + "';";
            return (ObtenerParticipantes(consulta)[0]);
        }

        public bool AlmacenarParticipante(ClienteModel participante)
        {
            string consulta = "INSERT INTO Participante ";
            string columnas = "(  correoParticipantePK, nombre, apellido1, apellido2, genero, pais, fechaNacimiento, nivelEducativo )";
            string valores = "( @correoParticipantePK, @nombre, @apellido1, @apellido2, @genero, @pais, @fechaNacimiento, @nivelEducativo );";
            consulta += columnas + " VALUES " + valores;

            Dictionary<string, object> valoresParametros = new Dictionary<string, object>()
            {
                { "@correoParticipantePK", participante.correo },
                { "@nombre", participante.nombre },
                { "@apellido1", participante.apellido1 },
                { "@apellido2", participante.apellido2 },
                { "@genero", participante.genero },
                { "@pais", participante.pais },
                { "@fechaNacimiento", participante.fechaNacimiento },
                { "@nivelEducativo" , participante.nivelEducativo }
            };

            bool exito = InsertarEnBaseDatos(consulta, valoresParametros);

            return exito;
        }

        public bool ParticipanteEstaAlmacenado(string correo)
        {
            string consulta = "SELECT 1 AS 'Inscrito' FROM Participante WHERE correoParticipantePK = '" + correo + "';";
            DataTable resultado = LeerBaseDeDatos(consulta);
            try
            {
                return (Convert.ToInt32(resultado.Rows[0]["Inscrito"]) == 1);
            }
            catch
            {
                return (false);
            }
        }

        public bool AlmacenarParticipacion(string correo, string nombreActividad, double precio)
        {
            string consulta = "INSERT INTO Factura (pagoTotal, correoParticipanteFK, nombreActividadFK) VALUES (@pagoTotal, @correoParticipanteFK, @nombreActividadFK)";
            Dictionary<string, object> valoresParametros = new Dictionary<string, object>()
            {
                { "@nombreActividadFK", nombreActividad },
                { "@correoParticipanteFK", correo },
                { "@pagoTotal", precio }
            };

            bool exito = InsertarEnBaseDatos(consulta, valoresParametros);

            return exito;
        }
        public string AppendRestante(string toAppend, int comprobacion)
        {
            if (comprobacion < 10)
            {
                toAppend = "0" + toAppend;
            }
            return toAppend;
        }
        public string TranslateFecha(string fecha)
        {
            DateTime fechaDateTime = DateTime.Parse(fecha);
            string year = AppendRestante(fechaDateTime.Year.ToString(), fechaDateTime.Year);
            string month = AppendRestante(fechaDateTime.Month.ToString(), fechaDateTime.Month);
            string day = AppendRestante(fechaDateTime.Day.ToString(), fechaDateTime.Day);

            string resultado = year + '-' + month + '-' + day;
            return resultado;
        }

    }
}