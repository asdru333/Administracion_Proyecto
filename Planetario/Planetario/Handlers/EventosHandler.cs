using System;
using System.Collections.Generic;
using System.Data;
using Planetario.Models;

namespace Planetario.Handlers
{
    public class EventosHandler: BaseDatosHandler
    {
        public bool InsertarEvento(EventoModel evento)
        {
            bool exito;
            string Consulta = "INSERT INTO Evento (nombreEventoPK, fecha, descripcion, hora, link)"
                + " VALUES (@titulo, @fecha, @descripcion, @hora, @link);";
            if (evento.Link == null) { evento.Link = ""; }
            Dictionary<string, object> valoresParametros = new Dictionary<string, object> {
                {"@titulo", evento.Titulo },
                {"@fecha", evento.Fecha },
                {"@descripcion", evento.Descripcion },
                {"@hora", evento.Hora },
                {"@link", evento.Link }
            };

            exito = InsertarEnBaseDatos(Consulta, valoresParametros);

            return exito;
        }

        public List<EventoModel> ObtenerTodosEventos()
        {
            List<EventoModel> eventos = new List<EventoModel>();
            string Consulta = "SELECT nombreEventoPK, CAST(fecha AS DATE) AS Fecha, descripcion, hora FROM Evento";
            DataTable tablaResultado = LeerBaseDeDatos(Consulta);
            foreach (DataRow columna in tablaResultado.Rows)
            {
                eventos.Add(
                    new EventoModel
                    {
                        Titulo = Convert.ToString(columna["nombreEventoPK"]),
                        Fecha = Convert.ToString(columna["Fecha"]).Split()[0],
                        Descripcion = Convert.ToString(columna["descripcion"]),
                        Hora = Convert.ToString(columna["hora"])
                    });
            }
            return eventos;
        }

        public EventoModel ObtenerUnEvento(string titulo)
        {
            EventoModel evento = null;
            string Consulta = "SELECT * FROM Evento WHERE nombreEventoPk = '" + titulo + "';";
            DataTable tablaResultado = LeerBaseDeDatos(Consulta);
            if (tablaResultado.Rows.Count >= 1)
            {
                evento = new EventoModel
                {
                    Titulo = Convert.ToString(tablaResultado.Rows[0]["nombreEventoPk"]),
                    Fecha = Convert.ToString(tablaResultado.Rows[0]["fecha"]).Split()[0],
                    Descripcion = Convert.ToString(tablaResultado.Rows[0]["descripcion"]),
                    Hora = Convert.ToString(tablaResultado.Rows[0]["hora"]),
                    Link = Convert.ToString(tablaResultado.Rows[0]["link"])
                };
            }
            return evento;
        }
    }
}