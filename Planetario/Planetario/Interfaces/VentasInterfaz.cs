using Planetario.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Planetario.Interfaces
{
    public interface VentasInterfaz
    {
        List<ComprableModel> ObtenerTodasLasEntradasDelCarrito(string correoUsuario);
        List<ComprableModel> ObtenerTodosLosProductosDelCarrito(string correoUsuario);
        bool EliminarDelCarrito(string correoUsuario, int idComprable);
        bool DisminiuirLaCantidadDelElementoDelCarrito(string correoUsuario, int idComprable);
        bool AumentarLaCantidadDelElementoDelCarrito(string correoUsuario, int idComprable);
        bool AgregarAlCarrito(int productID, int cantidad);
        double ObtenerPrecioTotalDeProductosDelCarrito(string correoUsuario);
        double ObtenerPrecioTotalDeEntradasDelCarrito(string correoUsuario);
        int ObtenerCantidadDeProductosDelCarrito(string correoUsuario);
        int ObtenerCantidadDeEntradasDelCarrito(string correoUsuario);
        int ObtenerCantidadDeProductoEspecifico(int idProducto);
        ComprableModel ObtenerComprable(int id);
    }
}