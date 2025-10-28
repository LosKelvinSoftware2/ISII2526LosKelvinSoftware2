namespace AppForSEII2526.API.DTO.Alquilar_Herramienta
{
    public class AlquilerDTO
    {

        public AlquilerDTO(String direccionEnvio, DateTime fechaAlquiler, DateTime fechaFin, float precioTotal, List<AlquilarItemDTO> alquilarItemDTOs, String nombreCliente, String apellidoCliente)
        {
            this.direccionEnvio = direccionEnvio;
            this.fechaAlquiler = fechaAlquiler;
            this.fechaFin = fechaFin;
            this.precioTotal = precioTotal;
            AlquilarItemDTOs = alquilarItemDTOs;
            this.nombreCliente = nombreCliente;
            this.apellidoCliente = apellidoCliente;
        }
        [Required]
        public String direccionEnvio { get; set; }
        [Required]
        public DateTime fechaAlquiler { get; set; }
        [Required]
        public DateTime fechaFin { get; set; }
        [Range(0, float.MaxValue)]
        public float precioTotal { get; set; }

        public String nombreCliente { get; set; }
        public String apellidoCliente { get; set; }



        public IList<AlquilarItemDTO> AlquilarItemDTOs { get; set; }
    }
}


