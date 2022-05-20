using System.Collections.Generic;

namespace Planetario.Models
{
    public class CompraModel
    {
        public ClienteModel Participante { get; set; }
        public TarjetaModel Tarjeta { get; set; }
        public List<ProductoModel> Productos { get; set; }
        public FacturaModel Factura { get; set; }
    }
}