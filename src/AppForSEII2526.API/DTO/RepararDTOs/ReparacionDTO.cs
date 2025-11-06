namespace AppForSEII2526.API.DTO.RepararDTOs
{
    public class ReparacionDTO
    {
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
    }
}
