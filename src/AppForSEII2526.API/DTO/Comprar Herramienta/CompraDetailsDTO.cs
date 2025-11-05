namespace AppForSEII2526.API.DTO.Comprar_Herramienta
{
    public class CompraDetailsDTO : CompraDTO
    {

        public CompraDetailsDTO(int id, ApplicationUser cliente, string direccionEnvio, DateTime fechaCompra, float precioTotal,
            List<CompraItemDTO> compraItemsDTO, tiposMetodoPago metodoPago):
            base (cliente, direccionEnvio, precioTotal, compraItemsDTO, metodoPago)
        {
            Id = id;    
            this.fechaCompra = fechaCompra;
        }
        
        [Key]
        public int Id { get; set; }

        // Vinculación con el usuario (cliente autenticado)
        public ApplicationUser Cliente { get; set; }

        [Required]
        public DateTime fechaCompra { get; set; }


        public override bool Equals(object? obj)
        {
            return obj is CompraDetailsDTO dTO &&
                   base.Equals(obj) &&
                   PrecioTotal == dTO.PrecioTotal &&
                   Id == dTO.Id &&
                   CompararFechas(fechaCompra, dTO.fechaCompra);
        }



        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, fechaCompra);
        }


    }
}
