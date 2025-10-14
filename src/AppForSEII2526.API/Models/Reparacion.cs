namespace AppForSEII2526.API.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Reparacion
    {
        [Key]
        public int Id { get; set; }

        //Vinculación con el usuario (cliente autenticado)
        [Required]
        public ApplicationUser Cliente { get; set; }


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


    }

}