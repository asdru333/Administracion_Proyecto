using System;
using System.Collections.Generic;
using System.Data;
using Planetario.Models;
using System.Web;
using Planetario.Interfaces;

namespace Planetario.Handlers
{
    public class ActividadHandler : BaseDatosHandler, ActividadesInterfaz
    {
        private List<ActividadModel> ConvertirTablaALista(DataTable tabla)
        {
            List<ActividadModel> actividades = new List<ActividadModel>();
            foreach (DataRow columna in tabla.Rows)
            {
                actividades.Add(
                    new ActividadModel
                    {
                        NombreActividad = Convert.ToString(columna["nombreActividadPK"]),
                        Descripcion = Convert.ToString(columna["descripcion"]),
                        Duracion = Convert.ToInt32(columna["duracionMins"]),
                        Complejidad = Convert.ToString(columna["complejidad"]),
                        PrecioAproximado = Convert.ToDouble(columna["precioAprox"]),
                        Categoria = Convert.ToString(columna["categoriaActividad"]),
                        DiaSemana = Convert.ToString(columna["diaSemana"]),
                        Fecha = Convert.ToString(columna["fechaActividad"]).Split()[0],
                        PropuestoPor = Convert.ToString(columna["propuestoPorFK"]),
                        PublicoDirigido = Convert.ToString(columna["publicoDirigidoActividad"]),
                        Tipo = Convert.ToString(columna["tipo"]),
                        Link = Convert.ToString(columna["link"])
                    });
            }
            return actividades;
        }

        private List<ActividadModel> ObtenerActividades(string consulta)
        {
            DataTable tabla = LeerBaseDeDatos(consulta);
            List<ActividadModel> lista = ConvertirTablaALista(tabla);
            return lista;
        }

        public List<ActividadModel> ObtenerTodasLasActividades()
        {
            string consulta = "SELECT * FROM Actividad";
            return (ObtenerActividades(consulta));
        }

        public List<ActividadModel> ObtenerActividadesAprobadas()
        {
            string consulta = "SELECT * FROM Actividad WHERE aprobado = 1";
            return (ObtenerActividades(consulta));
        }

        public List<ActividadModel> ObtenerActividadesRecomendadas(string publicoDirigido, string complejidad)
        {
            string consulta = "SELECT * FROM Actividad WHERE aprobado = 1 AND publicoDirigidoActividad = '" + publicoDirigido + "'AND complejidad = '" + complejidad + "';"; ;
            return (ObtenerActividades(consulta));
        }

        public List<ActividadModel> ObtenerActividadesPorBusqueda(string busqueda)
        {
            string consulta = "SELECT * FROM Actividad WHERE nombreActividadPK LIKE '%" + busqueda + "%' OR categoriaActividad LIKE '%" + busqueda + "%' OR tipo LIKE '%" + busqueda + "%';";
            return (ObtenerActividades(consulta));
        }

        public ActividadModel ObtenerActividad(string nombre)
        {
            string consulta = "Select * FROM Actividad WHERE nombreActividadPK = '" + nombre + "';";
            return (ObtenerActividades(consulta)[0]);
        }

        public bool InsertarEntrada(EntradaModel entrada, int cantidadEntradasInicial, string nombreActividad)
        {
            string consultaTablaComprable = "INSERT INTO Comprable (nombre, precio, cantidadDisponible) " +
                                           "VALUES (@nombre, @precio, @cantidadDisponible);";

            string consultaTablaEntrada = "DECLARE @identity int=IDENT_CURRENT('Comprable');" +
                                           "INSERT INTO Entrada " +
                                           "VALUES ( @identity, @nombreActividadFK); ";

            Dictionary<string, object> parametrosComprable = new Dictionary<string, object> {
                {"@nombre", entrada.Nombre },
                {"@precio", entrada.Precio },
                {"@cantidadDisponible", entrada.CantidadDisponible }
            };

            Dictionary<string, object> parametrosEntrada = new Dictionary<string, object> {
                {"@nombreActividadFK", entrada.NombreActividad }
            };

            return (InsertarEnBaseDatos(consultaTablaComprable, parametrosComprable) && InsertarEnBaseDatos(consultaTablaEntrada, parametrosEntrada));

        }

        public bool InsertarActividad(ActividadModel actividad)
        {
            string consulta =
                "INSERT INTO Actividad (nombreActividadPK, descripcion, " +
                "duracionMins, complejidad, precioAprox, categoriaActividad, fechaActividad, propuestoPorFK, publicoDirigidoActividad, tipo, link) " +
                " VALUES ( @nombreActividadPK, @descripcion, @duracionMins, @complejidad, " +
                "@precioAprox, @categoria, @fecha, @propuestoPorFK, @publicoDirigidoActividad, @tipo, @link)";

            if (actividad.Link == null) { actividad.Link = ""; }

            Dictionary<string, object> valoresParametros = new Dictionary<string, object> {
                {"@nombreActividadPK", actividad.NombreActividad },
                {"@descripcion", actividad.Descripcion },
                {"@duracionMins", actividad.Duracion },
                {"@complejidad", actividad.Complejidad },
                {"@precioAprox", actividad.PrecioAproximado },
                {"@categoria", actividad.Categoria },
                {"@fecha", actividad.Fecha},
                {"@propuestoPorFK", HttpContext.Current.User.Identity.Name },
                {"@publicoDirigidoActividad", actividad.PublicoDirigido },
                {"@tipo", actividad.Tipo },
                {"@link", actividad.Link }
            };
            return (InsertarEnBaseDatos(consulta, valoresParametros));
        }

        public bool InsertarTopico(string nombreActividad, string topico)
        {
            string consulta = "INSERT INTO ActividadTopicos (NombreActividadFK, topicosActividad) "
                + " VALUES (@NombreActividadFK, @topicosActividad)";

            Dictionary<string, object> valoresParametros = new Dictionary<string, object> {
                {"@NombreActividadFK", nombreActividad },
                {"@topicosActividad", topico }
            };
            return (InsertarEnBaseDatos(consulta, valoresParametros));
        }

        public List<string> ObtenerTopicosActividad(string nombre)
        {
            string consulta = "SELECT topicosActividad FROM ActividadTopicos WHERE nombreActividadFK = '" + nombre + "';";
            DataTable tablaResultados = LeerBaseDeDatos(consulta);
            List<string> topicos = new List<string>();

            foreach (DataRow fila in tablaResultados.Rows)
            {
                topicos.Add(Convert.ToString(fila["topicosActividad"]));
            }
            return topicos;
        }

        public int ObtenerEntradasDisponiblesPorActividad(string nombre)
        {
            int cantidad = 0;
            string consulta = "SELECT C.cantidadDisponible FROM Entrada E " +
                "JOIN Comprable C ON C.idComprablePK = E.idComprableFK " +
                "WHERE E.nombreActividadFK = '" + nombre + "';";

            DataTable tabla = LeerBaseDeDatos(consulta);
            foreach (DataRow columna in tabla.Rows)
            {
                cantidad = Convert.ToInt32(columna["cantidadDisponible"]);
            };  

            return cantidad;
        }

        private List<AsientoModel> ConvertirTablaAListaDeAsientos(DataTable tabla)
        {
            List<AsientoModel> asientos = new List<AsientoModel>();
            foreach (DataRow columna in tabla.Rows)
            {
                asientos.Add(
                    new AsientoModel
                    {
                        IdComprable = Convert.ToInt32(columna["idComprableFK"]),
                        Fila = Convert.ToInt32(columna["fila"]),
                        Columna = Convert.ToInt32(columna["columna"]),
                        Vendido = Convert.ToBoolean(columna["vendido"]),
                        Reservado = Convert.ToBoolean(columna["reservado"]),
                        FechaCompra = Convert.ToString(columna["fechaCompra"]),
                        CorreoParticipante = Convert.ToString(columna["correoParticipanteFK"])
                    });
            }
            return asientos;
        }

        public List<AsientoModel> ObtenerAsientos(string nombreActividad)
        {
            string consulta = " SELECT * FROM Asientos A JOIN " +
                "Entrada E on E.idComprableFK = A.idComprableFK " +
                "WHERE E.nombreActividadFK = '" + nombreActividad + "';";
            DataTable tabla = LeerBaseDeDatos(consulta);

            List<AsientoModel> asientos = ConvertirTablaAListaDeAsientos(tabla);
            return asientos;
        }

        public List<AsientoModel> ObtenerAsientosOcupados(string nombreActividad)
        {
            string consulta = " SELECT * FROM Asientos A JOIN " +
                "Entrada E on E.idComprableFK = A.idComprableFK " +
                "WHERE E.nombreActividadFK = '" + nombreActividad + "' AND ( A.reservado = 1 OR A.vendido = 1 );";
            DataTable tabla = LeerBaseDeDatos(consulta);

            List<AsientoModel> asientos = ConvertirTablaAListaDeAsientos(tabla);
            return asientos;
        }

        public bool AñadirAsientos(int cantidadFilas, int cantidadColumnas)
        {
            string consulta = "DECLARE @identity int= IDENT_CURRENT('Entrada');";

            consulta += "INSERT INTO Asientos VALUES ";
            for (int filas = 0; filas < cantidadFilas; filas++) {
                for(int columnas = 0; columnas < cantidadColumnas; columnas++)
                {
                    consulta += "( @identity, " + filas + ", " + columnas + ", 0, 0, NULL, NULL),";
                }                                    
            };
            consulta = consulta.Remove(consulta.Length - 1);
            consulta += ";";
            return InsertarEnBaseDatos(consulta, null);
        }

        public bool ActualizarReservarAsiento(int fila, int columna, string correo, bool reservado, string nombreActividad)
        {
            string consulta = "DECLARE @id INT;" +
                "SET @id = (SELECT TOP 1 A.idComprableFK FROM Asientos A JOIN Entrada E on E.idComprableFK = A.idComprableFK " +
                "WHERE E.nombreActividadFK = 'Analizar muestras de Marte');"+
                "UPDATE Asientos SET reservado = @reservado, correoParticipanteFK = @correoParticipanteFK " +
                "WHERE fila = @fila AND columna = @columna;";

            Dictionary<string, object> parametrosReserva = new Dictionary<string, object> {
                {"@nombreActividad", nombreActividad },
                {"@fila"   , fila },
                {"@columna" , columna},
                {"@correoParticipanteFK", correo},
                {"@reservado", reservado }
            };

            return ActualizarEnBaseDatos(consulta, parametrosReserva);
        }

        public bool VenderAsiento(int fila, int columna)
        {
            string consulta = "UPDATE Asientos SET vendido = 1" +
                "WHERE fila = @fila AND columna = @columna ;";

            Dictionary<string, object> parametrosVenta = new Dictionary<string, object> {
                {"@fila"   , fila },
                {"@columna" , columna},
            };

            return ActualizarEnBaseDatos(consulta, parametrosVenta);
        }

    }
}