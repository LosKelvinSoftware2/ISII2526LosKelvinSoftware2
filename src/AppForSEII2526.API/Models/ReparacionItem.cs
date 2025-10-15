namespace AppForSEII2526.API.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [PrimaryKey(nameof(herramientaId), nameof(reparacionId))]
    public class ReparacionItem
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Cantidad { get; set; }
        // Campo opcional
        public string Descripcion { get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        public float Precio { get; set; }

        // Relación con Herramienta
        public int herramientaId { get; set; }
        public Herramienta Herramienta { get; set; }

        // Relación con Reparacion
        public int reparacionId { get; set; }
        public Reparacion Reparacion { get; set; }

    }
}