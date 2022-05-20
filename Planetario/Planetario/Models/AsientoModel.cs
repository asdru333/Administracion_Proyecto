namespace Planetario.Models
{
    public class AsientoModel
    {
        public int IdComprable { get; set; }
        public int Fila { get; set; }
        public int Columna { get; set; }
        public bool Vendido { get; set; }
        public bool Reservado { get; set; }
        public string FechaCompra { get; set; }
        public string CorreoParticipante { get; set; }
    }
}