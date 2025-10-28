namespace AppForSEII2526.API.DTO.Comprar_Herramienta
{
    public class CompraItemDTO
    {
        public CompraItemDTO(int cantidad, string descripcion, float precio,
            int herramientaId, Herramienta herramienta, int compraId, Compra compra) {

            this.cantidad = cantidad;
            this.descripcion = descripcion;
            this.precio = precio;
            this.herramientaId = herramientaId;
            this.herramienta = herramienta;
            this.compraId = compraId;
            this.compra = compra;

        }

      

        [Required]
        public int cantidad { get; set; }

        [Required]
        [MaxLength(300)]
        public string descripcion { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "El precio no puede ser negativo.")]
        public float precio { get; set; }

        // Conexión con Compra y Herramienta (base de datos)

        public int compraId { get; set; }
        public Compra compra { get; set; }

        public int herramientaId { get; set; }
        public Herramienta herramienta { get; set; }




    }
}
