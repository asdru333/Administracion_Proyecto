using System;
using System.Collections.Generic;
using System.Data;
using Planetario.Interfaces;

namespace Planetario.Handlers
{
    public class ReportesHandler : BaseDatosHandler, ReportesInterfaz
    {
        private List<string> ObtenerLista(string consulta, string opcion)
        {
            DataTable tabla = LeerBaseDeDatos(consulta);
            List<string> lista = ConvertirTablaALista(tabla, opcion);
            return lista;
        }

        private List<string> ConvertirTablaALista(DataTable tabla, string opcion)
        {
            List<string> lista = new List<string>();
            foreach (DataRow columna in tabla.Rows)
            {
                lista.Add(Convert.ToString(columna[opcion]));
            }
            return lista;
        }

        public List<Object> ObtenerTodosLosProductosFiltradosPorRanking(string fechaInicio, string fechaFinal, string orden)
        {
            string consulta = "SELECT DISTINCT nombre, idComprablePK, precio, fechaIngreso, fechaUltimaVenta, SUM(FC.cantidadComprada) as 'cantidadVendidos' " +
                              "FROM Producto P JOIN Comprable C ON idComprablePK = idComprableFK " +
                              "JOIN FacturaComprables FC ON C.idComprablePK = FC.idComprableFK " +
                              "JOIN Factura F ON FC.idFacturaFK = F.idFacturaPK " +
                              "WHERE DATEDIFF(DAY, '" + fechaInicio + "', F.fechaCompra) >= 0 " +
                              "AND DATEDIFF(DAY, '" + fechaFinal + "', F.fechaCompra ) <= 0 " +
                              "GROUP BY nombre, idComprablePK, precio, fechaIngreso, fechaUltimaVenta, cantidadVendidos " +
                              "ORDER BY SUM(FC.cantidadComprada) " + orden + ";";

            List<Object> info = new List<Object>();
            DataTable tabla = LeerBaseDeDatos(consulta);
            foreach (DataRow columna in tabla.Rows)
            {
                info.Add(new
                {
                    Nombre = Convert.ToString(columna["nombre"]),
                    Precio = Convert.ToDouble(columna["precio"]),
                    FechaIngreso = Convert.ToString(columna["fechaIngreso"]),
                    FechaUltimaVenta = Convert.ToString(columna["fechaUltimaVenta"]),
                    CantidadVendidos = Convert.ToInt32(columna["cantidadVendidos"])
                });
            }
            return info;
        }

        public List<string> ObtenerTodosLosProductosFiltradosPorCategoriaFechasVentas(string nombre, string fechaInicio, string fechaFinal)
        {
            string consulta = consultaProductosFiltradosPorCategoria(nombre, fechaInicio, fechaFinal);

            string opcion = "fechaCompra";
            return ObtenerLista(consulta, opcion);
        }

        public List<int> ObtenerTodosLosProductosFiltradosPorCategoriaCantidadVentas(string nombre, string fechaInicio, string fechaFinal)
        {
            string consulta = consultaProductosFiltradosPorCategoria(nombre, fechaInicio, fechaFinal);

            DataTable tabla = LeerBaseDeDatos(consulta);
            List<int> ventas = new List<int>();
            foreach (DataRow columna in tabla.Rows)
            {
                ventas.Add(Convert.ToInt32(columna["cantidadComprada"]));
            }
            return ventas;
        }

        private string consultaProductosFiltradosPorCategoria(string nombre, string fechaInicio, string fechaFinal)
        {
            string consulta = "WITH TABLA_FECHAS AS (" +
                              "SELECT FORMAT(F.fechaCompra, 'd') as 'fechaCompra', FC.cantidadComprada " +
                              "FROM Producto P JOIN Comprable C ON C.idComprablePK = P.idComprableFK " +
                              "JOIN FacturaComprables FC " +
                              "ON C.idComprablePK = FC.idComprableFK " +
                              "JOIN Factura F " +
                              "ON FC.idFacturaFK = F.idFacturaPK " +
                              "WHERE C.nombre = '" + nombre + "' " +
                              "AND DATEDIFF(DAY, '" + fechaInicio + "', F.fechaCompra) >= 0 " +
                              "AND DATEDIFF(DAY, '" + fechaFinal + "', F.fechaCompra) <= 0)" +
                              "SELECT sum(cantidadComprada) as 'cantidadComprada', fechaCompra " +
                              "FROM TABLA_FECHAS " +
                              "GROUP BY fechaCompra " +
                              "ORDER BY CAST(fechaCompra as date) ASC";
            return consulta;
        }

        public List<string> ObtenerTodasLasCategorias()
        {
            string consulta = "SELECT DISTINCT categoria " +
                              "FROM Producto P JOIN Comprable C " +
                              "ON idComprablePK = idComprableFK " +
                              "ORDER BY categoria ASC;";

            string opcion = "categoria";
            return ObtenerLista(consulta, opcion);
        }

        public List<string> ObtenerTodosLosProductos()
        {
            string consulta = "SELECT DISTINCT C.nombre " +
                              "FROM Producto P JOIN Comprable C " +
                              "ON idComprablePK = idComprableFK " +
                              "ORDER BY C.nombre ASC;";

            string opcion = "nombre";
            return ObtenerLista(consulta, opcion);
        }

        public List<object> ConsultaPorCategoriasPersonaExtranjeras(string categoria)
        {
            string consulta = "SELECT SUM(FC.cantidadComprada) as 'cantidad', Pe.pais, C.nombre, C.precio " +
                              "FROM Producto Pr JOIN Comprable C " +
                              "ON C.idComprablePK = Pr.idComprableFK " +
                              "JOIN FacturaComprables FC " +
                              "ON C.idComprablePK = FC.idComprableFK " +
                              "JOIN Factura F " +
                              "ON FC.idFacturaFK = F.idFacturaPK " +
                              "JOIN Persona Pe " +
                              "ON F.correoPersonaFK = Pe.correoPersonaPK " +
                              "WHERE Pr.categoria = '" + categoria + "' " +
                              "AND Pe.pais != 'Costa Rica' " +
                              "GROUP BY Pe.Pais, C.nombre, C.precio";

            DataTable tablaResultados = LeerBaseDeDatos(consulta);
            List<object> info = new List<object>();

            foreach (DataRow fila in tablaResultados.Rows)
            {
                info.Add( new 
                { 
                    Nombre = Convert.ToString(fila["nombre"]),
                    Pais = Convert.ToString(fila["pais"]),
                    Precio = Convert.ToDouble(fila["precio"]),
                    Cantidad = Convert.ToString(fila["cantidad"]),
                    Ingresos = (Convert.ToDouble(fila["precio"]) * Convert.ToDouble(fila["cantidad"]))
                });
            }

            return info;
        }

        public List<object> ConsultaPorCategoriaProductoGeneroEdad(string categoria, string genero, string publico)
        {
            string consulta = "SELECT SUM(FC.cantidadComprada) as 'cantidad', C.nombre, C.precio " +
                              "FROM Producto Pr JOIN Comprable C " +
                              "ON C.idComprablePK = Pr.idComprableFK " +
                              "JOIN FacturaComprables FC " +
                              "ON C.idComprablePK = FC.idComprableFK " +
                              "JOIN Factura F " +
                              "ON FC.idFacturaFK = F.idFacturaPK " +
                              "JOIN Persona Pe " +
                              "ON F.correoPersonaFK = Pe.correoPersonaPK " +
                              "WHERE Pr.categoria = '" + categoria + "' " +
                              "AND Pe.genero = '" + genero + "' " +
                              "AND dbo.UFN_CategoriaPorEdad(DATEDIFF(YEAR, Pe.fechaNacimiento, GETDATE())) = '" + publico + "' " +
                              "GROUP BY C.nombre, C.precio;";

            DataTable tablaResultados = LeerBaseDeDatos(consulta);
            List<object> info = new List<object>();

            foreach (DataRow fila in tablaResultados.Rows)
            {
                info.Add(new
                {
                    Nombre = Convert.ToString(fila["nombre"]),
                    Precio = Convert.ToDouble(fila["precio"]),
                    Cantidad = Convert.ToString(fila["cantidad"]),
                    Ingresos = (Convert.ToDouble(fila["precio"]) * Convert.ToDouble(fila["cantidad"]))
                });
            }

            return info;
        }

        public List<object> ConsultaProductosCompradosJuntos(string publico, string membresia)
        {
            string consulta = "EXEC USP_PRODUCTOS_COMPRADOS_JUNTOS '" + membresia + "', '" + publico + "'";

            DataTable tablaResultados = LeerBaseDeDatos(consulta);
            List<object> info = new List<object>();

            foreach (DataRow fila in tablaResultados.Rows)
            {
                int vecesCompradosJuntos = Convert.ToInt32(fila["VecesCompradosJuntos"]);
                double precioProducto = Convert.ToDouble(fila["PrecioProducto"]);
                double precioCompradoCon = Convert.ToDouble(fila["PrecioCompradoCon"]);
                info.Add(new
                {
                    Producto = Convert.ToString(fila["Producto"]),
                    CompradoCon = Convert.ToString(fila["ProductoCompradoCon"]),
                    CantidadVeces = vecesCompradosJuntos,
                    Ingresos = (vecesCompradosJuntos * (precioProducto+precioCompradoCon))
                });
            }
            return info;
        }
    }
}