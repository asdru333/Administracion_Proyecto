using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planetario.Interfaces
{
    public interface MembresiasInterfaz
    {
        string ObtenerMembresia(string correoUsuario);
        bool ActualizarMembresia(string correoUsuario, string membresia);
    }
}