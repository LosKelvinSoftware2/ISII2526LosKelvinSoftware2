namespace AppForSEII2526.API.Models
{
    public class Alquiler
    {
        [Required]
        public String direccionEnvio { get; set; }
        public String fechaAlquiler { get; set; }
        public String fechaFin { get; set; }
        public int Id { get; set; }
        public String periodo { get; set; }
        [Range(0, float.MaxValue)]
        public int precioTotal { get; set; }

        public List<AlquilarItem> AlquilarItems { get; set; }
        [Required]
        public tiposMetodoPago MetodoPago { get; set; }
    }
}
