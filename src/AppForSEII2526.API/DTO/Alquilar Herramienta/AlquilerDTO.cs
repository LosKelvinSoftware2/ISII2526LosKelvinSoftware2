namespace AppForSEII2526.API.DTO.Alquilar_Herramienta
{
    public class AlquilerDTO
    {
       
        public AlquilerDTO(String direccionEnvio, String fechaAlquiler, String fechaFin, String periodo, float precioTotal, ApplicationUser cliente, List<AlquilarItemDTO> alquilarItemDTOs)
        {
            this.direccionEnvio = direccionEnvio;
            this.fechaAlquiler = fechaAlquiler;
            this.fechaFin = fechaFin;
            this.periodo = periodo;
            this.precioTotal = precioTotal;
            Cliente = cliente;
            AlquilarItemDTOs = alquilarItemDTOs;
        }
        [Required]
        public String direccionEnvio { get; set; }
        [Required]
        public String fechaAlquiler { get; set; }
        [Required]
        public String fechaFin { get; set; }
        public String periodo { get; set; }
        [Range(0, float.MaxValue)]
        public float precioTotal { get; set; }

        //Vinculación con el usuario (cliente autenticado)
        [Required]
        public ApplicationUser Cliente { get; set; }

        public IList<AlquilarItemDTO> AlquilarItemDTOs { get; set; }
    }
}
