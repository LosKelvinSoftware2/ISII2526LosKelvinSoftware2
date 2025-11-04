namespace AppForSEII2526.API.DTO.RepararDTOs
{
    public class ReparacionDTO
    {
        private object value;

        
        public ReparacionDTO(int id, string nombreCliente, string apellidosCliente, DateTime fechaEntrega, DateTime fechaRecogida, float precioTotal, List<ReparacionItemDTO> itemsReparacion, tiposMetodoPago metodoPago)
        {
            Id = id;
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            FechaEntrega = fechaEntrega;
            FechaRecogida = fechaRecogida;
            PrecioTotal = precioTotal;
            ItemsReparacion = itemsReparacion;
            MetodoPago = metodoPago;
        }

        [Key]
        public int Id { get; set; }

        //Vinculación con el usuario (cliente autenticado)
        // Datos del cliente
        [Required]
        [MaxLength(100)]
        public string NombreCliente { get; set; }

        [Required]
        [MaxLength(100)]
        public string ApellidosCliente { get; set; }


        [Required]
        public DateTime FechaEntrega { get; set; } // No puede ser anterior a hoy

        [Required]
        public DateTime FechaRecogida { get; set; }

        [Required]
        [Range(0, float.MaxValue)]
        public float PrecioTotal { get; set; }

        // Relación con los ítems de reparación
        public List<ReparacionItemDTO> ItemsReparacion { get; set; }

        // Método de pago obligatorio
        [Required]
        public tiposMetodoPago MetodoPago { get; set; }
    }
}
