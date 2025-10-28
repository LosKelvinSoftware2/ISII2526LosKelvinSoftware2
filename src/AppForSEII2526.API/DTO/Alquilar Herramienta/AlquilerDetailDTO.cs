namespace AppForSEII2526.API.DTO.Alquilar_Herramienta
{
    public class AlquilerDetailDTO
    {
        public AlquilerDetailDTO(int id, string cliente, string direccionEnvio,
            DateTime fechaAlquiler, DateTime fechaDevolucion, float precioTotal,
            List<AlquilarItemDTO> alquilarItems)
        {
            Id = id;
            Cliente = cliente;
            this.direccionEnvio = direccionEnvio;
            this.fechaAlquiler = fechaAlquiler;
            this.fechaDevolucion = fechaDevolucion;
            PrecioTotal = precioTotal;
            AlquilarItems = alquilarItems;
        }
        public int Id { get; set; }
        public string Cliente { get; set; }
        public string direccionEnvio { get; set; }
        public DateTime fechaAlquiler { get; set; }
        public DateTime fechaDevolucion { get; set; }
        public float PrecioTotal { get; set; }
        public List<AlquilarItemDTO> AlquilarItems { get; set; }
    }
}
