using System.Linq;
namespace AppForSEII2526.API.DTO.Alquilar_Herramienta
{
    public class AlquilerDetailDTO : AlquilerDTO
    {
        public AlquilerDetailDTO(int id, DateTime fechaAlquiler, string nombreCliente, string apellidoCliente, string direccionEnvio, float precioTotal,
            DateTime fechaFin, DateTime fechaInicio, List<AlquilarItemDTO> AlquilarItems, tiposMetodoPago MetodoPago) :
            base(nombreCliente, apellidoCliente, direccionEnvio, precioTotal, fechaFin, fechaInicio, AlquilarItems, MetodoPago)
        {
            Id = id;
            this.nombreCliente = nombreCliente;
            this.apellidoCliente = apellidoCliente;
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
                   fechaAlquiler == dTO.fechaAlquiler &&
                   AlquilarItems.SequenceEqual(dTO.AlquilarItems) &&
                   nombreCliente == dTO.nombreCliente &&
                   apellidoCliente == dTO.apellidoCliente;
        }
    }
}

