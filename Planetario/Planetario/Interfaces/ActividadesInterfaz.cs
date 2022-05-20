
using Planetario.Models;
using System.Collections.Generic;

namespace Planetario.Interfaces
{
    public interface ActividadesInterfaz
    {
        List<ActividadModel> ObtenerTodasLasActividades();

        List<ActividadModel> ObtenerActividadesAprobadas();

        List<ActividadModel> ObtenerActividadesRecomendadas(string publicoDirigido, string complejidad);

        List<ActividadModel> ObtenerActividadesPorBusqueda(string busqueda);

        ActividadModel ObtenerActividad(string nombre);

        bool InsertarEntrada(EntradaModel entrada, int cantidadEntradasInicial, string nombreActividad);

        bool InsertarActividad(ActividadModel actividad);

        bool InsertarTopico(string nombreActividad, string topico);

        List<string> ObtenerTopicosActividad(string nombre);

        int ObtenerEntradasDisponiblesPorActividad(string nombre);

        List<AsientoModel> ObtenerAsientos(string nombreActividad);

        List<AsientoModel> ObtenerAsientosOcupados(string nombreActividad);

        bool AñadirAsientos(int cantidadFilas, int cantidadColumnas);

        bool ActualizarReservarAsiento(int fila, int columna, string correo, bool reservado, string nombreActividad);

        bool VenderAsiento(int fila, int columna);

    }
    
}