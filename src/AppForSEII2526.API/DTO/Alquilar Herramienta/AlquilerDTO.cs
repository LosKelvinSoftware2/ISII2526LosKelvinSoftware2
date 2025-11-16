namespace AppForSEII2526.API.DTO.Alquilar_Herramienta
{
    public class AlquilerDTO
    {
        public AlquilerDTO(string nombreCliente, string apellidoCliente, string direccionEnvio, float precioTotal,
            DateTime fechaFin, DateTime fechaInicio, List<AlquilarItemDTO> AlquilarItems, tiposMetodoPago MetodoPago)
        {
            this.nombreCliente = nombreCliente;
            this.apellidoCliente = apellidoCliente;
            this.direccionEnvio = direccionEnvio;
            this.precioTotal = precioTotal;
            this.fechaFin = fechaFin;
            this.fechaInicio = fechaInicio;
            this.AlquilarItems = AlquilarItems;
            this.MetodoPago = MetodoPago;
        }
        public String nombreCliente { get; set; }
        public String apellidoCliente { get; set; }
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