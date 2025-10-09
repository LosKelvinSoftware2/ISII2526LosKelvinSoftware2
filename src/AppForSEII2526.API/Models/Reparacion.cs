namespace AppForSEII2526.API.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Reparacion
    {
        [Key]
        public int Id { get; set; }

        // Datos del cliente (obligatorios)
        [Required]
        [MaxLength(100)]
        public string NombreCliente { get; set; }

        [Required]
        [MaxLength(100)]
        public string ApellidoCliente { get; set; }

        // Opcional
        [Phone]
        public string? NumTelefono { get; set; }

        [Required]
        public DateTime FechaEntrega { get; set; } // No puede ser anterior a hoy

        [Required]
        public DateTime FechaRecogida { get; set; }

        [Required]
        [Range(0, float.MaxValue)]
        public float PrecioTotal { get; set; }

        // Relación con los ítems de reparación
        public List<ReparacionItem> ItemsReparacion { get; set; } = new();

        // Método de pago obligatorio
        [Required]
        public tiposMetodoPago MetodoPago { get; set; }

        // Relación con el usuario (cliente autenticado)
        [Required]
        public string ClienteId { get; set; }

        [ForeignKey(nameof(ClienteId))]
        public ApplicationUser Cliente { get; set; }
    }

}