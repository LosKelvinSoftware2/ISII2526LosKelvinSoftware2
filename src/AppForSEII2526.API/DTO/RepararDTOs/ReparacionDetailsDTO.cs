using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTO.RepararDTOs
{
    public class ReparacionDetailsDTO
    {
        public ReparacionDetailsDTO(int id,string nombreCliente,string apellidosCliente,DateTime fechaEntrega,DateTime fechaRecogida,float precioTotal,tiposMetodoPago metodoPago,List<ReparacionItemDTO> itemsReparacion)
        {
            Id = id;
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            FechaEntrega = fechaEntrega;
            FechaRecogida = fechaRecogida;
            PrecioTotal = precioTotal;
            MetodoPago = metodoPago;
            ItemsReparacion = itemsReparacion;
        }

        [Key]
        public int Id { get; set; }

        // Datos del cliente
        [Required]
        [MaxLength(100)]
        public string NombreCliente { get; set; }

        [Required]
        [MaxLength(100)]
        public string ApellidosCliente { get; set; }

        // Fechas
        [Required]
        public DateTime FechaEntrega { get; set; }

        [Required]
        public DateTime FechaRecogida { get; set; }

        // Totales
        [Required]
        [Range(0, float.MaxValue)]
        public float PrecioTotal { get; set; }

        [Required]
        public tiposMetodoPago MetodoPago { get; set; }

        // Lista de herramientas a reparar (detalladas)
        public List<ReparacionItemDTO> ItemsReparacion { get; set; }
    }
}
