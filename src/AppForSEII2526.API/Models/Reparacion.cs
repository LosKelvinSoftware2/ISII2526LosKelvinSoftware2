namespace AppForSEII2526.API.Models
{
    public class Reparacion
    {
        public int Id { get; set; }
        public string fechaEntréga { get; set; }
        public string fechaRecogida { get; set; }
        public string nombreCliente { get; set; }
        public string apellidoCliente { get; set; }
        public string numTelefono { get; set; }
        public float precioTotal { get; set; }
        //Conexiones otras tablas
        public List<ReparacionItem> itemsReparacion { get; set; }
        //public ApplicationUser cliente { get; set; }
    }
}