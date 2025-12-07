
namespace AppForSEII2526.API.DTO.Alquilar_Herramienta
{
    public class AlquilerHerramientasDTO
    {
        public AlquilerHerramientasDTO(int id, string nombre, string material, float precio, string fabricante)
        {
            Id = id;
            Nombre = nombre;
            Material = material;
            Precio = precio;
            this.Fabricante = fabricante;
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
        public string Fabricante { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AlquilerHerramientasDTO dTO &&
                   Id == dTO.Id &&
                   Nombre == dTO.Nombre &&
                   Material == dTO.Material &&
                   Precio == dTO.Precio &&
                   Fabricante == dTO.Fabricante;
        }
    }
}