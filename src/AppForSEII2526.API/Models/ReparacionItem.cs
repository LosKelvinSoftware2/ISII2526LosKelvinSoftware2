namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(herramientaId), nameof(IdReparacion))]
    public class ReparacionItem
    {
        public float precio { get; set; }
        public string descripcion { get; set; }
        public int cantidad { get; set; }

        //Conexiones otras tablas
        public Herramienta herramienta { get; set; }
        public int herramientaId { get; set; }
        public Reparacion IdReparacion { get; set; }
        public int reparacion { get; set; }
    }
}