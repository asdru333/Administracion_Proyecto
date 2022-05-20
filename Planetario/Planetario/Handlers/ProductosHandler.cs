using Planetario.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Planetario.Interfaces;

namespace Planetario.Handlers
{
    public class ProductosHandler: BaseDatosHandler, ProductosInterfaz
    {
        private readonly ArchivosHandler ManejadorDeImagen = new ArchivosHandler();
        private static List<ProductoModel> ConvertirTablaProductoALista(DataTable tabla)
        {
            List<ProductoModel> productos = new List<ProductoModel>();
            foreach (DataRow columna in tabla.Rows)
            {
                productos.Add(
                new ProductoModel
                {
                    Id = Convert.ToInt32(columna["idComprablePK"]),
                    Nombre = Convert.ToString(columna["nombre"]),
                    Precio = Convert.ToDouble(columna["precio"]),
                    CantidadDisponible = Convert.ToInt32(columna["cantidadDisponible"]),
                    CantidadRebastecer = Convert.ToInt32(columna["cantidadRebastecer"]),
                    Tamano = Convert.ToString(columna["tamano"]),
                    Categoria = Convert.ToString(columna["categoria"]),
                    Descripcion = Convert.ToString(columna["descripcion"]),
                    FechaIngreso = Convert.ToString(columna["fechaIngreso"]),
                    FechaUltimaVenta = Convert.ToString(columna["fechaUltimaVenta"])
                });
            }
            return productos;
        }

        private List<ProductoModel> ObtenerProductos(string consulta)
        {
            DataTable tabla = LeerBaseDeDatos(consulta);
            List<ProductoModel> productos = ConvertirTablaProductoALista(tabla);
            return productos;
        }

        public ProductoModel ObtenerProducto(int id)
        {
            string consulta = "SELECT * FROM Producto P JOIN Comprable C ON P.idComprableFK = C.idComprablePK WHERE C.idComprablePK = '" + id.ToString() + "';";
            return (ObtenerProductos(consulta)[0]);
        }

        public List<ProductoModel> ObtenerProductosFiltrados(double precioMin, double precioMax, string categoria, string busqueda, string orden)
        {

            string consulta = "SELECT * FROM Producto P JOIN Comprable C ON P.idComprableFK = C.idComprablePK " +
            "WHERE Precio >= " + precioMin.ToString() + " AND Precio <= " + precioMax.ToString() + " ";
            if (categoria != "" && categoria != null)
            {
                consulta += "AND P.categoria = '" + categoria + "' ";
            }
            if (busqueda != "" && busqueda != null)
            {
                consulta += "AND nombre LIKE '%" + busqueda + "%' ";
            }
            consulta += "ORDER BY " + orden + ";";

            return ObtenerProductos(consulta);
        }

        public bool InsertarProducto(ProductoModel producto)
        {
            string consultaTablaComprable = "INSERT INTO Comprable (nombre, precio, cantidadDisponible) " +
                                            "VALUES (@nombre, @precio, @cantidadDisponible);";

            string consultaTablaProducto = "DECLARE @identity int=IDENT_CURRENT('Comprable');" +
                                           "INSERT INTO Producto (idComprableFK, cantidadRebastecer, tamano, categoria, descripcion, fechaIngreso, fechaUltimaVenta, fotoArchivo, fotoTipo, cantidadVendidos) " +
                                           "VALUES ( @identity, @cantidadRebastecer, @tamano, @categoria, @descripcion, @fechaIngreso, NULL, @fotoArchivo, @fotoTipo, 0 ); ";

            Dictionary<string, object> parametrosComprable = new Dictionary<string, object> {
                {"@nombre", producto.Nombre },
                {"@precio", producto.Precio },
                {"@cantidadDisponible", producto.CantidadDisponible }
            };

            Dictionary<string, object> parametrosProducto = CrearDiccionarioParametrosDeProductos(producto);

            return (InsertarEnBaseDatos(consultaTablaComprable, parametrosComprable) && InsertarEnBaseDatos(consultaTablaProducto, parametrosProducto));
        }

        private Dictionary<string, object> CrearDiccionarioParametrosDeProductos(ProductoModel producto)
        {
            Dictionary<string, object> parametrosProducto = new Dictionary<string, object> {
                {"@cantidadRebastecer", producto.CantidadRebastecer },
                {"@tamano", producto.Tamano },
                {"@categoria", producto.Categoria },
                {"@descripcion", producto.Descripcion },
                {"@fechaIngreso", producto.FechaIngreso },
                {"@fotoTipo", producto.FotoArchivo.ContentType }
            };

            parametrosProducto.Add("@fotoArchivo", ManejadorDeImagen.ConvertirArchivoABytes(producto.FotoArchivo));
            return parametrosProducto;
        }

        public Tuple<byte[], string> ObtenerFoto(int id)
        {
            string columnaContenido = "fotoArchivo";
            string columnaTipo = "fotoTipo";
            string consulta = "SELECT " + columnaContenido + ", " + columnaTipo + " FROM Producto WHERE idComprableFK = @id";
            KeyValuePair<string, object> parametro = new KeyValuePair<string, object>("@id", id);
            return ObtenerArchivo(consulta, parametro, columnaContenido, columnaTipo);
        }
    }
}