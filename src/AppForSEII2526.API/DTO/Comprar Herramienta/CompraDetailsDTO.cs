using AppForSEII2526.API.DTO.Alquilar_Herramienta;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace AppForSEII2526.API.DTO.Comprar_Herramienta
{
    public class CompraDetailsDTO : CompraDTO
    {
        public CompraDetailsDTO(int id, DateTime fechaCompra, string nombreCliente, string apellidoCliente, 
            double telefonoCliente, string correoCliente, string direccionEnvio, float precioTotal,
            List<CompraItemDTO> CompraItems, tiposMetodoPago MetodoPago) :
            base(nombreCliente, apellidoCliente, telefonoCliente, correoCliente, direccionEnvio, 
                precioTotal, fechaCompra, CompraItems, MetodoPago)
        {
            Id = id;
        }

        [Key]
        int Id { get; set; }


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
