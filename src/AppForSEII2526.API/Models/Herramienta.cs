namespace AppForSEII2526.API.Models
{
    public class Herramienta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(50)]
        public string Material { get; set; }

        [Required]
        [Range(0, float.MaxValue)]
        public float Precio { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TiempoReparacion { get; set; } // días hábiles

        // Relación con Fabricante
        [Required]
        public int FabricanteId { get; set; }

        [ForeignKey(nameof(FabricanteId))]
        public Fabricante Fabricante { get; set; }

        // Relación con ReparacionItem
        public List<ReparacionItem> ReparacionItems { get; set; }
        // Relación con CompraItem
        public List<CompraItem> CompraItems { get; set; }
        // Relación con OfertaItem
        public List<OfertaItem> Ofertaitems { get; set; }
        // Relación con AlquilarItem
        public List<AlquilarItem> AlquilarItems { get; set; }
    }
    
}
