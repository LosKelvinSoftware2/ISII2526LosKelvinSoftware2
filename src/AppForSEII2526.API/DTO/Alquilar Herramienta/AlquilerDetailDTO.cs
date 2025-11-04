
namespace AppForSEII2526.API.DTO.Alquilar_Herramienta
{
    public class AlquilerDetailDTO : AlquilerDTO
    {
        public AlquilerDetailDTO(int id, DateTime fechaAlquiler, ApplicationUser cliente, string direccionEnvio, float precioTotal,
            DateTime fechaFin, DateTime fechaInicio, List<AlquilarItemDTO> alquilarItems) :
            base(cliente, direccionEnvio, precioTotal, fechaFin, fechaInicio, alquilarItems)
        {
            Id = id;
            this.fechaAlquiler = fechaAlquiler;
        }

        [Key]
        int Id { get; set; }
        [Required]
        public DateTime fechaAlquiler { get; set; }

        // Vinculación con el usuario (cliente autenticado)
        public ApplicationUser Cliente { get; set; }
        [Required]
        public String direccionEnvio { get; set; }

        [Range(0, float.MaxValue)]
        public float precioTotal { get; set; }
        [Required]
        public DateTime fechaFin { get; set; }

        [Required]
        public DateTime fechaInicio { get; set; }
        // Relación con Items de alquiler
        public List<AlquilarItemDTO> AlquilarItems { get; set; }

    }
}
