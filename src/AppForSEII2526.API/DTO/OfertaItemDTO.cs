namespace AppForSEII2526.API.DTO
{
    public class OfertaItemDTO
    {

        public OfertaItemDTO(float precioFinal, string nombre, string material,
            string fabricante, float precioOriginal)
        {
            this.precioFinal = precioFinal;
            this.nombre = nombre;
            this.material = material;
            this.fabricante = fabricante;
            this.precioOriginal = precioOriginal; // Viene de Herramienta.
            // Viene de OfertaItem.precioFinal (float se recomienda cambiar a decimal) 
        }

        
        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "El precio total no puede ser negativo.")]
        public float precioFinal { get; set; }

        string fabricante { get; set; }

        float precioOriginal;

        string nombre { get; set; }

        string material { get; set; }



        // FK hacia Herramienta
        public int herramientaId { get; set; }
        public Herramienta herramienta { get; set; }

        // FK hacia Reparacion
        public int ofertaId { get; set; }
        public Oferta oferta { get; set; }
    }
}
