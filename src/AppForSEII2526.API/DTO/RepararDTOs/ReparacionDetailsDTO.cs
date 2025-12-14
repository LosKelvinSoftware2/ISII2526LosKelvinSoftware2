using Humanizer;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTO.RepararDTOs
{
    public class ReparacionDetailsDTO : ReparacionDTO
    {
        public ReparacionDetailsDTO() { }

        public ReparacionDetailsDTO(int id, string nombreCliente, string apellidosCliente, string? numTelefono,
                                  DateTime fechaEntrega, DateTime fechaRecogida, float precioTotal,
                                  tiposMetodoPago metodoPago, List<ReparacionItemDTO> itemsReparacion) :
                                base(nombreCliente, apellidosCliente, numTelefono, fechaEntrega, metodoPago, itemsReparacion)
        {
            Id = id;
            FechaEntrega = fechaEntrega;
            FechaRecogida = fechaRecogida;
            PrecioTotal = precioTotal;
            MetodoPago = metodoPago;
        }

        [Key]
        public int Id { get; set; }
        public DateTime FechaRecogida { get; set; }
        public float PrecioTotal { get; set; }

        

        public override bool Equals(object? other)
        {
            return other is ReparacionDetailsDTO dTO &&
                   base.Equals(other) &&
                   Id == dTO.Id &&
                   FechaRecogida == dTO.FechaRecogida &&
                   PrecioTotal == dTO.PrecioTotal;
        }

        // Propiedad adicional para mantener compatibilidad con el DTO de items específico
        //public List<ReparacionItemDTO> ItemsReparacionDetails { get; set; }


    }

    //public class ReparacionItemDetailsDTO
    //{
    //    public ReparacionItemDetailsDTO(string nombreHerramienta, float precio, int cantidad, string descripcion)
    //    {
    //        NombreHerramienta = nombreHerramienta;
    //        Precio = precio;
    //        Cantidad = cantidad;
    //        Descripcion = descripcion;
    //    }

    //    public string NombreHerramienta { get; set; }
    //    public float Precio { get; set; }
    //    public int Cantidad { get; set; }
    //    public string Descripcion { get; set; }

    //    public override bool Equals(object? obj)
    //    {
    //        return obj is ReparacionItemDetailsDTO dTO &&
    //               NombreHerramienta == dTO.NombreHerramienta &&
    //               Precio == dTO.Precio &&
    //               Cantidad == dTO.Cantidad &&
    //               Descripcion == dTO.Descripcion;
    //    }

    //}
}
