namespace AppForSEII2526.API.Models
{
    public class Herramienta
    {
        public int Id { get; set; }
        public string material { get; set; }
        public string nombre { get; set; }
        public float precio { get; set; }
        public string tiempoReparacion { get; set; }



        public List<OfertaItem> Ofertaitems { get; set; }
        public Fabricante Fabricante { get; set; }
        public int FabricanteId { get; set; }
        public List<CompraItem> CompraItems { get; set; }




    }
}
