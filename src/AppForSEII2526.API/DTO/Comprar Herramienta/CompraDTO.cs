namespace AppForSEII2526.API.DTO.Comprar_Herramienta
{
    public class CompraDTO
    {

        public CompraDTO(ApplicationUser cliente, string direccionEnvio, float PrecioTotal,
            DateTime fechaCompra, List<CompraItemDTO> CompraItemDTO, tiposMetodoPago MetodoPago)
        {
            this.cliente = cliente;
            this.direccionEnvio = direccionEnvio;
            this.PrecioTotal = PrecioTotal;
            this.MetodoPago = MetodoPago;
            this.CompraItems = CompraItemDTO;
            
        }

        //Vinculación con el usuario (cliente autenticado)
        [JsonIgnore]
        public ApplicationUser cliente { get; set; }

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
