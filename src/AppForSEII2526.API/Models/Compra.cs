namespace AppForSEII2526.API.Models
{
    public class Compra
    {
        public string apellidoCliente { get; set; }
        public string nombreCliente { get; set; }
        public string correoelectronico { get; set; }
        public string direccionEnvio { get; set; }
        public string fechacompra { get; set; }
        public int Id { get; set; }
        public float precioTotal { get; set; }
        public double telefono { get; set; }


        public List<CompraItem> CompraItems { get; set; }
        public tiposMetodoPago MetodoPago { get; set; }


    }
}
