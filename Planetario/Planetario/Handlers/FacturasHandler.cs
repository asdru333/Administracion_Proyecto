using System;
using System.Collections.Generic;
using System.Data;
using Planetario.Models;

namespace Planetario.Handlers
{
    public class FacturasHandler: BaseDatosHandler
    {
        public List<FacturaModel> ConvertirTablaALista(DataTable tabla)
        {
            List<FacturaModel> facturas = new List<FacturaModel>();
            foreach (DataRow columna in tabla.Rows)
            {
                facturas.Add(
                    new FacturaModel
                    {
                        id = Convert.ToInt32(columna["idFacturaPK"]),
                        fecha = Convert.ToString(columna["fechaCompra"]),
                        pago = Convert.ToDouble(columna["pagoTotal"]),
                        correoCliente = Convert.ToString(columna["correoParticipanteFK"]),
                        actividad = Convert.ToString(columna["nombreActividadFK"]),
                    });
            }
            return facturas;
        }

        public List<FacturaModel> ObtenerFacturas(string consulta)
        {
            DataTable tabla = LeerBaseDeDatos(consulta);
            List<FacturaModel> lista = ConvertirTablaALista(tabla);
            return lista;
        }

        public List<FacturaModel> ObtenerTodasLasFacturas()
        {
            string consulta = "SELECT * FROM Factura;";
            return (ObtenerFacturas(consulta));
        }

        public List<FacturaModel> ObtenerFacturasDeActividad(string nombreActividad)
        {
            string consulta = "EXEC USP_ObtenerTodosPagos '" + nombreActividad + "';";
            return (ObtenerFacturas(consulta));
        }

        public FacturaModel ObtenerFactura(int id)
        {
            string consulta = "SELECT * FROM Factura WHERE idFacturaPK ='" + id.ToString() + "';";
            return (ObtenerFacturas(consulta)[0]);
        }

        // funciones nuevas

        public void InsertarFactura(string correo,Dictionary<int,int> comprables)
        {
            string consultaFactura = "INSERT INTO Factura(correoPersonaFK,fechaCompra) VALUES(@correo,@fecha);";
            Dictionary<string, object> parametros = new Dictionary<string, object>()
            {
                {"@correo",correo },
                {"@fecha",DateTime.Now }
            };
            InsertarEnBaseDatosConIsolación(consultaFactura, parametros);

            string consultaFacturaComprables = "DECLARE @identity int=IDENT_CURRENT('Factura'); " +
                "INSERT INTO FacturaComprables VALUES(@identity,@idComprableFK,@cantidad);";

            Dictionary<string, object> parametrosFacturaComprables;
            foreach(KeyValuePair<int,int> comprable in comprables)
            {
                parametrosFacturaComprables = new Dictionary<string, object>()
                {
                    {"@idComprableFK", comprable.Key },
                    {"@cantidad", comprable.Value }
                };
                InsertarEnBaseDatosConIsolación(consultaFacturaComprables, parametrosFacturaComprables);
            }
        }
    }
}