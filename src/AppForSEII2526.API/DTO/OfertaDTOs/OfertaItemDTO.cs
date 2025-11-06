namespace AppForSEII2526.API.DTO.OfertaDTOs
{
    public class OfertaItemDTO
    {

        public OfertaItemDTO(int herramientaId, float precioFinal, string nombre, string material,
            string Fabricante, float precioOriginal)
        {
            this.herramientaId = herramientaId;
            this.precioFinal = precioFinal;
            this.nombre = nombre;
            this.material = material;
            this.Fabricante = Fabricante;
            this.precioOriginal = precioOriginal; // Viene de Herramienta.
            // Viene de OfertaItem.precioFinal (float se recomienda cambiar a decimal) 
        }

        
        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "El precio total no puede ser negativo.")]
        public float precioFinal { get; set; }

        public string Fabricante { get; set; }

        public float precioOriginal { get; set; }

        public string nombre { get; set; }

        public string material { get; set; }



        // FK hacia Herramienta
        public int herramientaId { get; set; }

        // FK hacia Reparacion
        public int ofertaId { get; set; }

    }
}
