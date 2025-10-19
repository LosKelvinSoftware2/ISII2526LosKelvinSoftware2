namespace AppForSEII2526.API.DTO
{
    public class HerramientaDTO
    {
        public HerramientaDTO(int id, string nombre, string material, 
            float precio, int tiemporep, Fabricante fabricante)
        {
            Id = id;
            Nombre = nombre;
            Material = material;
            Precio = precio;
            TiempoReparacion = tiemporep;
            this.fabricante = fabricante;
            ReparacionItems = new List<ReparacionItem>();
            CompraItems = new List<CompraItem>();
            Ofertaitems = new List<OfertaItem>();
            AlquilarItems = new List<AlquilarItem>();

        }


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
        public Fabricante fabricante { get; set; }
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
