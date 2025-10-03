namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(herramientaId), nameof(OfertaId))]
    public class OfertaItem
    {

        public float porcentaje { get; set; }
        public float precioFinal { get; set; }

        //Conexiones otras tablas
        public int idOferta { get; set; }

        // FK hacia Herramienta
        public int herramientaId { get; set; }
        public Herramienta herramienta { get; set; }

        // FK hacia Reparacion
        public int OfertaId { get; set; }
        public Oferta oferta { get; set; }

    }
}
