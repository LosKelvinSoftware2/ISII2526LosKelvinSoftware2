namespace AppForSEII2526.API.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Fabricante
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        // Relaciones
        public List<Herramienta> Herramientas { get; set; }
    }
}
