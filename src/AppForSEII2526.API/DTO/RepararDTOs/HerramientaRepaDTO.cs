using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTO.RepararDTOs
{
    public class HerramientaRepaDTO
    {
        public HerramientaRepaDTO(int id, string nombre, string material, string fabricante,float precio, int tiemporep)
        {
            Id = id;
            Nombre = nombre;
            Material = material;
            Precio = precio;
            Fabricante = fabricante;
            TiempoReparacion = tiemporep;            

        }


        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(50)]
        public string Material { get; set; }

        [Required, MaxLength(100)]
        public string Fabricante { get; set; }

        [Required]
        [Range(0, float.MaxValue)]
        public float Precio { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TiempoReparacion { get; set; } // días hábiles

    }
}
