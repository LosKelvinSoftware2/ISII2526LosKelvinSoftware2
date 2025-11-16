
namespace AppForSEII2526.API.DTO.OfertaDTOs
{
    public class HerramientaForOfertaDTO
    {
        public HerramientaForOfertaDTO(int id, string nombre, string material, 
            float precio, Fabricante fabricante)
        {
            Id = id;
            Nombre = nombre;
            Material = material;
            this.fabricante = fabricante;
            Precio = precio;
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

        // Relación con Fabricante
        public Fabricante fabricante { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is HerramientaForOfertaDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   Material == dTO.Material &&
                   Precio == dTO.Precio &&
                   fabricante.Id == dTO.fabricante.Id;
        }
        // Relación con ReparacionItem

    }
}
