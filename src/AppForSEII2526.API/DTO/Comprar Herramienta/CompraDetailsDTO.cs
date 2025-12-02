using AppForSEII2526.API.Models;
using Microsoft.CodeAnalysis.CSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;

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


        public override bool Equals(object obj)
        {
            if (obj is not CompraDetailsDTO other)
                return false;

            if (this.nombreCliente != other.nombreCliente ||
                this.apellidoCliente != other.apellidoCliente ||
                this.correoCliente != other.correoCliente ||
                this.direccionEnvio != other.direccionEnvio ||
                this.PrecioTotal != other.PrecioTotal ||
                this.MetodoPago != other.MetodoPago)
                return false;

            if(!CompararFechas(this.fechaCompra, other.fechaCompra))
                return false;

            if (this.CompraItems.Count != other.CompraItems.Count)
                return false;

            for (int i = 0; i < this.CompraItems.Count; i++)
            {
                if (this.CompraItems[i].nombre != other.CompraItems[i].nombre ||
                    this.CompraItems[i].cantidad != other.CompraItems[i].cantidad ||
                    this.CompraItems[i].precio != other.CompraItems[i].precio)
                {
                    return false;
                }
            }

            return true;
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, fechaCompra);
        }


    }
}