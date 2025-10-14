namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(herramientaId), nameof(ofertaId))]
    public class OfertaItem
    {

        [Required]
        [Range(0.0, 100.0)]
        public float porcentaje { get; set; }
        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "El precio total no puede ser negativo.")]
        public float precioFinal { get; set; }


        // FK hacia Herramienta
        [Required]
        [ForeignKey(nameof(herramientaId))]
        public int herramientaId { get; set; }

        // FK hacia Reparacion
        [Required]
        [ForeignKey(nameof(ofertaId))]
        public int ofertaId { get; set; }

    }
}
