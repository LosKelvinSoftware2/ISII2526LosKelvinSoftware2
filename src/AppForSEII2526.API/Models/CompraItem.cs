namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(herramientaId), nameof(compraId))]
    public class CompraItem
    {

        public int cantidad { get; set; }
        public string descripcion { get; set; }
        public float precio { get; set; }

        // Foreign Keys (Conexión a base de datos)
        public Compra compra { get; set; }
        public int compraId { get; set; }

        public Herramienta herramienta { get; set; }
        public int herramientaId { get; set; }



    }
}
