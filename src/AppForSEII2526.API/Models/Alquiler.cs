namespace AppForSEII2526.API.Models
{
    public class Alquiler
    {
        public String apellidoCliente { get; set; }
        public String correo { get; set; }
        public String direccionEnvio { get; set; }
        public String fechaAlquiler { get; set; }
        public String fechaFin { get; set; }
        public int Id { get; set; }
        public String nombreCliente { get; set; }
        public double numeroTelefono { get; set; }
        public String periodo { get; set; }
        public int precioTotal { get; set; }

        public List<AlquilarItem> AlquilarItems { get; set; }
        public tiposMetodoPago MetodoPago { get; set; }
    }
}
