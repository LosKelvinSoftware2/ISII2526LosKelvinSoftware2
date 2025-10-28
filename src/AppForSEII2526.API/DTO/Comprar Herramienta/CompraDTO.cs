namespace AppForSEII2526.API.DTO.Comprar_Herramienta
{
    public class CompraDTO
    {

        public CompraDTO(ApplicationUser Cliente, string direccionEnvio, float PrecioTotal, 
            List<CompraItemDTO> CompraItemDTO, tiposMetodoPago MetodoPago)
        {
            this.Cliente = Cliente;
            this.direccionEnvio = direccionEnvio;
            this.PrecioTotal = PrecioTotal;
            this.CompraItemsDTO = CompraItemDTO;
            this.MetodoPago = MetodoPago;
        }

        //Vinculación con el usuario (cliente autenticado)
        [Required]
        public ApplicationUser Cliente { get; set; }

        [Required]
        [MaxLength(200)]
        public string direccionEnvio { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "El precio total no puede ser negativo.")]
        public float PrecioTotal { get; set; }

        [Required]
        public tiposMetodoPago MetodoPago { get; set; }

        public List<CompraItemDTO> CompraItemsDTO { get; set; }




        //Métodos

        protected bool CompararFechas(DateTime fecha1, DateTime fecha2)
        {
            return (fecha1.Subtract(fecha2) < new TimeSpan(0, 1, 0));
        }
    }
}
