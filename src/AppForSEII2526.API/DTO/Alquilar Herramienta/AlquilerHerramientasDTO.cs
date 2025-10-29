namespace AppForSEII2526.API.DTO.Alquilar_Herramienta
{
    public class AlquilerHerramientasDTO
    { 
        public AlquilerHerramientasDTO(int id, string nombre, string material, float precio, Fabricante fabricante ,
            DateTime fechaInicio , DateTime fechaFin , string direccionEnvio, tiposMetodoPago metodoPago , string nombreCliente, string apellidoCliente)
        {
            Id = id;
            Nombre = nombre;
            Material = material;
            Precio = precio;
            this.fabricante = fabricante;
            AlquilerItems = new List<AlquilarItem>();
            this.fechaInicio = fechaInicio;
            this.fechaFin = fechaFin;
            this.direccionEnvio = direccionEnvio;
            this.metodoPago = metodoPago;
            this.nombreCliente = nombreCliente;
            this.apellidoCliente = apellidoCliente;
        }
        [Key]
        public int Id { get; set; }
        [StringLength(50 , ErrorMessage = "El nombre debe tener menos de 50 caracteres.")]
        public String Nombre { get; set; }
        public String Material { get; set; }
        public float Precio { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public string direccionEnvio { get; set; }
        public tiposMetodoPago metodoPago { get; set; }
        public string nombreCliente { get; set; }
        public string apellidoCliente { get; set; }
        //Relación con fabricante
        public Fabricante fabricante { get; set; }
        //Relación con AlquilerItem
        public List<AlquilarItem> AlquilerItems { get; set; }
    }
}
