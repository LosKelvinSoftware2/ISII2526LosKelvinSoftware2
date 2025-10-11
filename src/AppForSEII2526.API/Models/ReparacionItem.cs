namespace AppForSEII2526.API.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [PrimaryKey(nameof(HerramientaId), nameof(ReparacionId))]
    public class ReparacionItem
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Cantidad { get; set; }
        // Campo opcional
        public string? Descripcion { get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        public float Precio { get; set; }
        // Relación con Herramienta
        [Required]
        public int HerramientaId { get; set; }
        [ForeignKey(nameof(HerramientaId))]
        public Herramienta Herramienta { get; set; }
        // Relación con Reparacion
        [Required]
        public int ReparacionId { get; set; }
        [ForeignKey(nameof(ReparacionId))]
        public Reparacion Reparacion { get; set; }
    }
}