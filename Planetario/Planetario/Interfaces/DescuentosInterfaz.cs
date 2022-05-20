using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Planetario.Models;

namespace Planetario.Interfaces
{
    public interface DescuentosInterfaz
    {
        List<DescuentoModel> ObtenerTodosDescuentos(string codigo);
        int ObtenerPorcentajeDescuento(string codigo);
        bool InsertarDescuento(DescuentoModel descuento);
        bool EliminarDescuento(string codigo);
    }
}