namespace AppForSEII2526.API.Models
{
    public class Alquiler
    {
        [Required]
        public String direccionEnvio { get; set; }
        [Required]
        public String fechaAlquiler { get; set; }
        [Required]
        public String fechaFin { get; set; }
        [Key]
        public int Id { get; set; }
        public String periodo { get; set; }
        [Range(0, float.MaxValue)]
        public float precioTotal { get; set; }

        public List<AlquilarItem> AlquilarItems { get; set; }
        [Required]
        public tiposMetodoPago MetodoPago { get; set; }
    }
}
