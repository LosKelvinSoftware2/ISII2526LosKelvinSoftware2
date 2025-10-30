namespace AppForSEII2526.API.Models
{
    public class Alquiler
    {
        
        [Required]
        public String direccionEnvio { get; set; }
        [Required]
        public DateTime fechaAlquiler { get; set; }
        [Required]
        public DateTime fechaFin { get; set; }
        [Key]
        public int Id { get; set; }
        [Range(0, float.MaxValue)]
        public float precioTotal { get; set; }

        public string nombreCliente { get; set; }
        public string apellidoCliente { get; set; }

        //Vinculación con el usuario (cliente autenticado)
        public ApplicationUser Cliente { get; set; }

        [Required]
        public IList<AlquilarItem> AlquilarItems { get; set; }
        [Required]
        public tiposMetodoPago MetodoPago { get; set; }
    }
}
