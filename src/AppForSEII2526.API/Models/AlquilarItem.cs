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
        public int precio { get; set; }
        //FK hacia Alquiler
        [Required]
        public int alquilerId { get; set; }
        [ForeignKey(nameof(alquilerId))]
        public Alquiler alquiler { get; set; }
        //FK hacia Herramienta
        [Required]
        public int herramientaId { get; set; }
        [ForeignKey(nameof(herramientaId))]
        public Herramienta herramienta { get; set; }
    }
}
