namespace AppForSEII2526.API.Models
{
    public class Fabricante
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        //Conexiones otras tablas
        public List<Herramienta> herramientas { get; set; }



    }
}
