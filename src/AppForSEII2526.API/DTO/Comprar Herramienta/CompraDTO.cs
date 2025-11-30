namespace AppForSEII2526.API.DTO.Comprar_Herramienta
{
    public class CompraDTO
    {

        // Constructor vacío necesario para deserialización
        public CompraDTO()
        {

        }

        public CompraDTO(string nombreCliente, string apellidoCliente, double telefonoCliente, string correoCliente,
            string direccionEnvio, float PrecioTotal,
            DateTime fechaCompra, List<CompraItemDTO> CompraItemDTO, tiposMetodoPago MetodoPago)
        {
            this.nombreCliente = nombreCliente;
            this.apellidoCliente = apellidoCliente;
            this.fechaCompra = fechaCompra;
            this.telefonoCliente = telefonoCliente;
            this.correoCliente = correoCliente;
            this.direccionEnvio = direccionEnvio;
            this.PrecioTotal = PrecioTotal;
            this.MetodoPago = MetodoPago;
            this.CompraItems = CompraItemDTO;

        }

        //Vinculación con el usuario (cliente autenticado)

        [Required]
        public string nombreCliente { get; set; }

        [Required]
        public string apellidoCliente { get; set; }

        public double telefonoCliente { get; set; }

        public string correoCliente { get; set; }

        [Required]
        [MaxLength(200)]
        public string direccionEnvio { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "El precio total no puede ser negativo.")]
        public float PrecioTotal { get; set; }

        [Required]
        public DateTime fechaCompra { get; set; }

        [Required]
        public tiposMetodoPago MetodoPago { get; set; }

        public List<CompraItemDTO> CompraItems { get; set; }



        //Métodos

        protected bool CompararFechas(DateTime fecha1, DateTime fecha2)
        {
            return (fecha1.Subtract(fecha2) < new TimeSpan(0, 1, 0));
        }
    }
}