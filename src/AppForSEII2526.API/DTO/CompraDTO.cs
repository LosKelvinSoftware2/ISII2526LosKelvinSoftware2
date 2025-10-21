namespace AppForSEII2526.API.DTO
{
    public class CompraDTO
    {

        public CompraDTO(int Id, ApplicationUser Cliente, string direccionEnvio, DateTime fechaCompra, float PrecioTotal, 
            List<CompraItemDTO> CompraItemDTO, tiposMetodoPago MetodoPago)
        {
            this.Id = Id;
            this.Cliente = Cliente;
            this.direccionEnvio = direccionEnvio;
            this.fechaCompra = fechaCompra;
            this.PrecioTotal = PrecioTotal;
            this.CompraItemsDTO = CompraItemDTO;
            this.MetodoPago = MetodoPago;
        }

        [Key]
        public int Id { get; set; }

        //Vinculación con el usuario (cliente autenticado)
        [Required]
        public ApplicationUser Cliente { get; set; }

        [Required]
        [MaxLength(200)]
        public string direccionEnvio { get; set; }

        [Required]
        public DateTime fechaCompra { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "El precio total no puede ser negativo.")]
        public float PrecioTotal { get; set; }

        [Required]
        public tiposMetodoPago MetodoPago { get; set; }

        public List<CompraItemDTO> CompraItemsDTO { get; set; }


    }
}
