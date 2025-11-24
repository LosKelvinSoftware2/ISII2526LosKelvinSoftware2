namespace AppForSEII2526.API.DTO.Comprar_Herramienta
{
    public class CompraItemDTO
    {
        public CompraItemDTO(int herramientaId, string nombre, string material, int cantidad, float precio)
        {
            this.herramientaId = herramientaId;
            this.nombre = nombre;
            this.material = material;
            this.cantidad = cantidad;
            this.precio = precio;

        }

        // FK hacia Herramienta
        [Required]
        public int herramientaId { get; set; }
        [Required]
        public string nombre { get; set; }
        [Required]
        public string material { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad no puede ser menor de 1.")]
        public int cantidad { get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        public float precio { get; set; }

    }
}