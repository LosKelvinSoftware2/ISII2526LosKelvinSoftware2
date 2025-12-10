namespace AppForSEII2526.API.DTO.RepararDTOs
{
    public class ReparacionDTO
    {
        public ReparacionDTO() { }

        
        public ReparacionDTO(string userName, string nombreCliente, string apellidosCliente, string? numTelefono,
                           DateTime fechaEntrega, tiposMetodoPago metodoPago, List<ReparacionItemDTO> itemsReparacion)
        {
            UserName = userName;
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            NumTelefono = numTelefono;
            FechaEntrega = fechaEntrega;
            MetodoPago = metodoPago;
            ItemsReparacion = itemsReparacion;
        }

        public ReparacionDTO(string nombreCliente, string apellidosCliente, string? numTelefono, DateTime fechaEntrega, tiposMetodoPago metodoPago, List<ReparacionItemDTO> itemsReparacion)
        {
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            NumTelefono = numTelefono;
            FechaEntrega = fechaEntrega;
            MetodoPago = metodoPago;
            ItemsReparacion = itemsReparacion;
        }

        [Required, MaxLength(100)]
        public string UserName { get; set; }  // Nuevo campo para buscar el usuario

        [Required, MaxLength(100)]
        public string NombreCliente { get; set; }

        [Required, MaxLength(100)]
        public string ApellidosCliente { get; set; }

        [Phone]
        public string? NumTelefono { get; set; }

        [Required]
        public DateTime FechaEntrega { get; set; }

        [Required]
        public tiposMetodoPago MetodoPago { get; set; }

        [Required]
        public List<ReparacionItemDTO> ItemsReparacion { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReparacionDTO dTO &&
                   UserName == dTO.UserName &&
                   NombreCliente == dTO.NombreCliente &&
                   ApellidosCliente == dTO.ApellidosCliente &&
                   NumTelefono == dTO.NumTelefono &&
                   FechaEntrega == dTO.FechaEntrega &&
                   MetodoPago == dTO.MetodoPago &&
                   ItemsReparacion.SequenceEqual(dTO.ItemsReparacion);
        }
    }
}
