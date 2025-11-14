
namespace AppForSEII2526.API.DTO.Alquilar_Herramienta
{
    public class AlquilerDetailDTO : AlquilerDTO
    {
        public AlquilerDetailDTO(int id, DateTime fechaAlquiler, ApplicationUser cliente, string direccionEnvio, float precioTotal,
            DateTime fechaFin, DateTime fechaInicio, List<AlquilarItemDTO> AlquilarItems, tiposMetodoPago MetodoPago) :
            base(cliente, direccionEnvio, precioTotal, fechaFin, fechaInicio, AlquilarItems, MetodoPago)
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
            return obj is AlquilerDetailDTO dTO &&
                   direccionEnvio == dTO.direccionEnvio &&
                   precioTotal == dTO.precioTotal &&
                   fechaFin == dTO.fechaFin &&
                   fechaInicio == dTO.fechaInicio &&
                   MetodoPago == dTO.MetodoPago &&
                   Id == dTO.Id &&
                   nombreCliente == dTO.nombreCliente &&
                   apellidoCliente == dTO.apellidoCliente &&
                   fechaAlquiler == dTO.fechaAlquiler;
        }
    }
}
