namespace AppForSEII2526.API.DTO
{
    public class AlquilerDTO
    {
        public AlquilerDTO(string direccionEnvio, string fechaAlquiler, string fechaFin, int id, string periodo, float precioTotal, ApplicationUser cliente, List<AlquilarItemDTO> alquilarItemDTOs, tiposMetodoPago metodoPago)
        {
            this.direccionEnvio = direccionEnvio;
            this.fechaAlquiler = fechaAlquiler;
            this.fechaFin = fechaFin;
            Id = id;
            this.periodo = periodo;
            this.precioTotal = precioTotal;
            Cliente = cliente;
            AlquilarItemDTOs = alquilarItemDTOs;
            MetodoPago = metodoPago;
        }

        [Required]
        public String direccionEnvio { get; set; }
        [Required]
        public String fechaAlquiler { get; set; }
        [Required]
        public String fechaFin { get; set; }
        [Key]
        public int Id { get; set; }
        public String periodo { get; set; }
        [Range(0, float.MaxValue)]
        public float precioTotal { get; set; }

        //Vinculación con el usuario (cliente autenticado)
        [Required]
        public ApplicationUser Cliente { get; set; }

        [Required]
        public List<AlquilarItem> AlquilarItems { get; set; }
        [Required]
        public tiposMetodoPago MetodoPago { get; set; }
        public List<AlquilarItemDTO> AlquilarItemDTOs { get; }
    }
}
