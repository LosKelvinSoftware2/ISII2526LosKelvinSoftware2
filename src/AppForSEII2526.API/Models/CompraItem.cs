namespace AppForSEII2526.API.Models
{

    [PrimaryKey(nameof(herramientaId), nameof(compraId))]
    public class CompraItem
    {
        [Required]
        public int cantidad { get; set; }

        [Required]
        [MaxLength(300)]
        public string descripcion { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "El precio no puede ser negativo.")]
        public float precio { get; set; }

        // Foreign Keys (Conexión a base de datos)

        public int compraId { get; set; }
        public Compra compra { get; set; }

        public int herramientaId { get; set; }
        public Herramienta herramienta { get; set; }



        }
}       
