namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(herramientaId), nameof(ofertaId))]
    public class OfertaItem
    {

        [Required(ErrorMessage = "Por favor, introduce el porcentaje")]
        [Range(0.0, 100.0, ErrorMessage = "Error! El porcentaje debe estar entre 0 y 100")]
        public float? porcentaje { get; set; }
        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "El precio total no puede ser negativo.")]
        public float precioFinal { get; set; }


        // FK hacia Herramienta
        public int herramientaId { get; set; }
        public Herramienta herramienta { get; set; }

        // FK hacia Reparacion
        public int ofertaId { get; set; }
        public Oferta oferta { get; set; }

    }
}
