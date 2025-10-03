namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(herramientaId), nameof(ReparacionId))]
    public class ReparacionItem
    {
        public float precio { get; set; }
        public string descripcion { get; set; }
        public int cantidad { get; set; }

        //Conexiones otras tablas
        // FK hacia Herramienta
        public int herramientaId { get; set; }
        public Herramienta herramienta { get; set; }

        // FK hacia Reparacion
        public int ReparacionId { get; set; }
        public Reparacion Reparacion { get; set; }
    }
}