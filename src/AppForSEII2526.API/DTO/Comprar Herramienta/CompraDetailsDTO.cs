using AppForSEII2526.API.DTO.Alquilar_Herramienta;

namespace AppForSEII2526.API.DTO.Comprar_Herramienta
{
    public class CompraDetailsDTO : CompraDTO
    {

        public CompraDetailsDTO(int id, DateTime fechaCompra, ApplicationUser cliente, string direccionEnvio, float precioTotal,
            List<CompraItemDTO> CompraItems, tiposMetodoPago MetodoPago) :
            base(cliente, direccionEnvio, precioTotal, fechaCompra, CompraItems, MetodoPago)
        {
            Id = id;
            nombreCliente = cliente.Nombre;
            apellidoCliente = cliente.Apellido;
            this.fechaAlquiler = fechaAlquiler;
        }

        [Key]
        int Id { get; set; }
        [Required]
        public string nombreCliente { get; set; }
        [Required]
        public string apellidoCliente { get; set; }
        [Required]
        public DateTime fechaAlquiler { get; set; }


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
