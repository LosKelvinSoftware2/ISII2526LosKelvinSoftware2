namespace AppForSEII2526.API.Models
{
    public class Alquiler
    {
        public Alquiler(string direccionEnvio, DateTime fechaAlquiler, DateTime fechaFin, int id, float precioTotal, IList<AlquilarItem> alquilarItems, tiposMetodoPago metodoPago , string nombreCliente , string apellidoCliente)
        {
            this.direccionEnvio = direccionEnvio;
            this.fechaAlquiler = fechaAlquiler;
            this.fechaFin = fechaFin;
            Id = id;
            this.precioTotal = precioTotal;
            AlquilarItems = alquilarItems;
            MetodoPago = metodoPago;
            this.nombreCliente= nombreCliente;
            this.apellidoCliente= apellidoCliente;
        }

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
