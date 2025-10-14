namespace AppForSEII2526.API.Models
{

    public class CompraItem
    {
        [Required]
        public int cantidad { get; set; }

        [Required]
        [MaxLength(300)]
        public string descripcion { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "El precio no puede ser negativo.")]
        public float precio { get; set; }

        // Foreign Keys (Conexión a base de datos)

        [ForeignKey(nameof(compraId))]
        public int compraId { get; set; }

        [ForeignKey(nameof(herramientaId))] 
        public int herramientaId { get; set; }



    }
}       
