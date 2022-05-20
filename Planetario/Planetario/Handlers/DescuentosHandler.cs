using Planetario.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Planetario.Interfaces;


namespace Planetario.Handlers
{
    public class DescuentosHandler: BaseDatosHandler, DescuentosInterfaz
    {
        private List<DescuentoModel> ConvertirTablaAListaDescuento(DataTable tabla)
        {
            List<DescuentoModel> productos = new List<DescuentoModel>();
            foreach (DataRow columna in tabla.Rows)
            {
                productos.Add(
                new DescuentoModel
                {
                    Codigo = Convert.ToString(columna["codigoDescuentoPK"]),
                    Descuento = Convert.ToInt32(columna["porcentajeDescuento"]),
                });
            }
            return productos;
        }

        public List<DescuentoModel> ObtenerTodosDescuentos(string codigo)
        {
            string consulta = "SELECT * FROM Descuento WHERE codigoDescuentoPK";
            DataTable tabla = LeerBaseDeDatos(consulta);
            List<DescuentoModel> descuento = ConvertirTablaAListaDescuento(tabla);
            return descuento;
        }

        public int ObtenerPorcentajeDescuento(string codigo)
        {           
            string consulta = "SELECT * FROM Descuento WHERE codigoDescuentoPK = '" + codigo + "';";
            DataTable tabla = LeerBaseDeDatos(consulta);
            List<DescuentoModel> descuento = ConvertirTablaAListaDescuento(tabla);
            int porcentaje = 0;
            if (descuento.Count != 0)
            {
                porcentaje = descuento[0].Descuento;
            }
            return porcentaje;
        }

        public bool InsertarDescuento(DescuentoModel descuento)
        {
            string consulta = "INSERT INTO Descuento (codigoDescuentoPK, porcentajeDescuento) " +
                                "VALUES (@codigo, @porcentaje)";
            Dictionary<string, object> parametrosProducto = new Dictionary<string, object> {
                {"@codigo"   , descuento.Codigo },
                {"@porcentaje"   , descuento.Descuento }
            };
            return (InsertarEnBaseDatos(consulta, parametrosProducto));
        }

        public bool EliminarDescuento(string codigo)
        {
            string consulta = "DELETE FROM Descuento WHERE codigoDescuentoPK = '"+codigo+"';";
            return EliminarEnBaseDatos(consulta, null);
        }
    }
}