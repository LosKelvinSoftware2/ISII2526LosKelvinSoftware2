using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTO.RepararDTOs
{
    public class ReparacionDetailsDTO
    {
        public ReparacionDetailsDTO(int id, string nombreCliente, string apellidosCliente, string? numTelefono, DateTime fechaEntrega, DateTime fechaRecogida, float precioTotal, tiposMetodoPago metodoPago, List<ReparacionItemDetailsDTO> itemsReparacion)
        {
            Id = id;
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            NumTelefono = numTelefono;
            FechaEntrega = fechaEntrega;
            FechaRecogida = fechaRecogida;
            PrecioTotal = precioTotal;
            MetodoPago = metodoPago;
            ItemsReparacion = itemsReparacion;
        }

        public int Id { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidosCliente { get; set; }
        public string? NumTelefono { get; set; }
        public DateTime FechaEntrega { get; set; }
        public DateTime FechaRecogida { get; set; }
        public float PrecioTotal { get; set; }
        public tiposMetodoPago MetodoPago { get; set; }

        public List<ReparacionItemDetailsDTO> ItemsReparacion { get; set; }
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
    }
}
