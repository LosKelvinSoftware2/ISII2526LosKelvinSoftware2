namespace AppForSEII2526.API.Models
{
    public class Alquiler
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public String direccionEnvio { get; set; }
        [Required]
        public DateTime fechaAlquiler { get; set; }
        [Required]
        public DateTime fechaFin { get; set; }

        [Required]
        public DateTime fechaInicio { get; set; }

        public int periodo { get; set; }
        [Range(0, float.MaxValue)]
        public float precioTotal { get; set; }

        //Vinculación con el usuario (cliente autenticado)
        public ApplicationUser Cliente { get; set; }

        [Required]
        public List<AlquilarItem> AlquilarItems { get; set; }
        [Required]
        public tiposMetodoPago MetodoPago { get; set; }
    }
}