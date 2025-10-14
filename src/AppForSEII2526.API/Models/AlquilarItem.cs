namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(herramientaId), nameof(alquilerId))]
    public class AlquilarItem
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int cantidad { get; set; }
        [Required]
        [Range (0, float.MaxValue)]
        public float precio { get; set; }
        //FK hacia Alquiler
        [Required]
        [ForeignKey(nameof(alquilerId))]
        public int alquilerId { get; set; }
        
        //FK hacia Herramienta
        [Required]
        [ForeignKey(nameof(herramientaId))]
        public int herramientaId { get; set; }
        
        
    }
}
