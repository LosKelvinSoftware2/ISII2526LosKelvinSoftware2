namespace AppForSEII2526.API.DTO.Alquilar_Herramienta
{
    public class AlquilerDTO
    {
        public AlquilerDTO(ApplicationUser cliente, string direccionEnvio, float precioTotal,
            DateTime fechaFin, DateTime fechaInicio, List<AlquilarItemDTO> alquilarItems)
        {
            Cliente = cliente;
            this.direccionEnvio = direccionEnvio;
            this.precioTotal = precioTotal;
            this.fechaFin = fechaFin;
            this.fechaInicio = fechaInicio;
            AlquilarItems = alquilarItems;
        }
        // Vinculación con el usuario (cliente autenticado)
        public ApplicationUser Cliente { get; set; }
        [Required]
        public String direccionEnvio { get; set; }

        [Range(0, float.MaxValue)]
        public float precioTotal { get; set; }
        [Required]
        public DateTime fechaFin { get; set; }

        [Required]
        public DateTime fechaInicio { get; set; }
        // Relación con Items de alquiler
        public List<AlquilarItemDTO> AlquilarItems { get; set; }
        public tiposMetodoPago MetodoPago { get; set; }
    }
}