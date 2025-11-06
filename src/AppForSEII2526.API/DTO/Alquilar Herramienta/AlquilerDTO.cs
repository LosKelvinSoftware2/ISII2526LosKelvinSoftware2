namespace AppForSEII2526.API.DTO.Alquilar_Herramienta
{
    public class AlquilerDTO
    {
        public AlquilerDTO(ApplicationUser cliente, string direccionEnvio, float precioTotal,
            DateTime fechaFin, DateTime fechaInicio, List<AlquilarItemDTO> AlquilarItems, tiposMetodoPago MetodoPago)
        {
            Cliente = cliente;
            this.direccionEnvio = direccionEnvio;
            this.precioTotal = precioTotal;
            this.fechaFin = fechaFin;
            this.fechaInicio = fechaInicio;
            this.AlquilarItems = AlquilarItems;
            this.MetodoPago = MetodoPago;
        }
        // Cliente
        [JsonIgnore] // No queremos que se muestre toda la info del cliente
        public ApplicationUser Cliente { get; set; }
        [Required]
        public String direccionEnvio { get; set; }

        [Range(0, float.MaxValue)]
        public float precioTotal { get; set; }
        [Required]
        public DateTime fechaFin { get; set; }

        [Required]
        public DateTime fechaInicio { get; set; }
        [JsonIgnore] // No queremos que se muestre el método de pago directamente
        public tiposMetodoPago MetodoPago { get; set; }
        // Relación con Items de alquiler
        public List<AlquilarItemDTO> AlquilarItems { get; set; }
    }
}