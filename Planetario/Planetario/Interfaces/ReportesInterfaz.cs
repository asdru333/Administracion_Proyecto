using Planetario.Models;
using System.Collections.Generic;

namespace Planetario.Interfaces
{
    public interface ReportesInterfaz
    {
        List<string> ObtenerTodasLasCategorias();

        List<string> ObtenerTodosLosProductos();

        List<object> ObtenerTodosLosProductosFiltradosPorRanking(string fechaInicio, string fechaFinal, string orden);

        List<string> ObtenerTodosLosProductosFiltradosPorCategoriaFechasVentas(string categoria, string fechaInicio, string fechaFinal);

        List<int> ObtenerTodosLosProductosFiltradosPorCategoriaCantidadVentas(string categoria, string fechaInicio, string fechaFinal);

        List<object> ConsultaPorCategoriasPersonaExtranjeras(string categoria);

        List<object> ConsultaPorCategoriaProductoGeneroEdad(string categoria, string genero, string publico);

        List<object> ConsultaProductosCompradosJuntos(string publico, string membresia);
    }
}