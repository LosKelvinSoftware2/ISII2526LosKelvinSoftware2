namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(herramientaId), nameof(alquilerId))]
    public class AlquilarItem
    {
        public int cantidad { get; set; }
        public int precio { get; set; }
        //FK hacia Alquiler
        public int alquilerId { get; set; }
        public Alquiler alquiler { get; set; }
        //FK hacia Herramienta
        public int herramientaId { get; set; }
        public Herramienta herramienta { get; set; }
    }
}
