namespace AppForSEII2526.API.DTO.RepararDTOs
{
    public class ReparacionDTO
    {
        public ReparacionDTO()
        {
            ItemsReparacion = new List<ReparacionItemDTO>();
        }

        public ReparacionDTO(string nombreC, string apellidos, string? numTelefono, tiposMetodoPago metodoPago,
            List<ReparacionItemDTO> herramientas)
        {
            this.NombreCliente = nombreC;
            this.ApellidosCliente = apellidos;
            this.NumTelefono = numTelefono;
            this.MetodoPago = metodoPago;
            ItemsReparacion = herramientas;
        }

        // Constructor para pruebas unitarias 
        public ReparacionDTO(string nombreC, string apellidos, string? numTelefono, tiposMetodoPago metodoPago,
        DateTime fechaEntrega, List<ReparacionItemDTO> herramientas)
        {
            this.NombreCliente = nombreC;
            this.ApellidosCliente = apellidos;
            this.NumTelefono = numTelefono;
            this.MetodoPago = metodoPago;
            this.FechaEntrega = fechaEntrega;
            ItemsReparacion = herramientas;
        }
        //constructor para el detalle heredar
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
        public List<ReparacionItemDTO> ItemsReparacion { get; set; } = new List<ReparacionItemDTO>();

        public override bool Equals(object? obj)
        {
            return obj is ReparacionDTO dTO &&
                   NombreCliente == dTO.NombreCliente &&
                   ApellidosCliente == dTO.ApellidosCliente &&
                   NumTelefono == dTO.NumTelefono &&
                   FechaEntrega == dTO.FechaEntrega &&
                   MetodoPago == dTO.MetodoPago &&
                   ItemsReparacion.SequenceEqual(dTO.ItemsReparacion);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(NombreCliente, ApellidosCliente, NumTelefono, MetodoPago, ItemsReparacion);
        }
    }
}
