using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTO.RepararDTOs
{
    public class ReparacionDetailsDTO : ReparacionDTO
    {
        public ReparacionDetailsDTO() { }

        public ReparacionDetailsDTO(int id, string nombreCliente, string apellidosCliente, string? numTelefono,
                                  DateTime fechaEntrega, DateTime fechaRecogida, float precioTotal,
                                  tiposMetodoPago metodoPago, List<ReparacionItemDetailsDTO> itemsReparacion)
        {
            Id = id;
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            NumTelefono = numTelefono;
            FechaEntrega = fechaEntrega;
            FechaRecogida = fechaRecogida;
            PrecioTotal = precioTotal;
            MetodoPago = metodoPago;
            ItemsReparacionDetails = itemsReparacion;
        }

        public int Id { get; set; }
        public DateTime FechaRecogida { get; set; }
        public float PrecioTotal { get; set; }

        // Propiedad adicional para mantener compatibilidad con el DTO de items específico
        public List<ReparacionItemDetailsDTO> ItemsReparacionDetails { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReparacionDetailsDTO dTO &&
                   Id == dTO.Id &&
                   NombreCliente == dTO.NombreCliente &&
                   ApellidosCliente == dTO.ApellidosCliente &&
                   NumTelefono == dTO.NumTelefono &&
                   FechaEntrega == dTO.FechaEntrega &&
                   FechaRecogida == dTO.FechaRecogida &&
                   PrecioTotal == dTO.PrecioTotal &&
                   MetodoPago == dTO.MetodoPago &&
                   EqualityComparer<List<ReparacionItemDetailsDTO>>.Default.Equals(ItemsReparacionDetails, dTO.ItemsReparacionDetails);
        }
    }

    public class ReparacionItemDetailsDTO
    {
        public ReparacionItemDetailsDTO(string nombreHerramienta, float precio, int cantidad, string descripcion)
        {
            NombreHerramienta = nombreHerramienta;
            Precio = precio;
            Cantidad = cantidad;
            Descripcion = descripcion;
        }

        public string NombreHerramienta { get; set; }
        public float Precio { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReparacionItemDetailsDTO dTO &&
                   NombreHerramienta == dTO.NombreHerramienta &&
                   Precio == dTO.Precio &&
                   Cantidad == dTO.Cantidad &&
                   Descripcion == dTO.Descripcion;
        }

    }
}
