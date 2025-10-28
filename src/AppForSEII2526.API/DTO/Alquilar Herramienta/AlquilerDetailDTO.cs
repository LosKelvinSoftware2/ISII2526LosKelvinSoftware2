
namespace AppForSEII2526.API.DTO.Alquilar_Herramienta
{
    public class AlquilerDetailDTO : AlquilerDTO

    {
        private List<AlquilarItem> alquilerItems;

        public AlquilerDetailDTO(int id, string nombreCliente, string apellidoCliente, string direccionEnvio, DateTime fechaAlquiler, DateTime fechaDevolucion, float precioTotal, List<AlquilarItemDTO> alquilarItems)
            : base(direccionEnvio, fechaAlquiler, fechaDevolucion, precioTotal, alquilarItems, nombreCliente, apellidoCliente)
        {
            this.Id = id;
        }

     

        public int Id { get; set; }
        public string nombreCliente { get; set; }
        public string apellidoCliente { get; set; }
        public string direccionEnvio { get; set; }
        public DateTime fechaAlquiler { get; set; }
        public DateTime fechaDevolucion { get; set; }
        public float PrecioTotal { get; set; }
        public List<AlquilarItemDTO> AlquilarItems { get; set; }
    }
}



