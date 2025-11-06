namespace AppForSEII2526.API.DTO.Comprar_Herramienta
{
    public class CompraItemDTO
    {
        public CompraItemDTO(Herramienta herramienta, int cantidad, float precio)
        {
            this.nombre = herramienta.Nombre;
            this.material = herramienta.Material;
            this.cantidad = cantidad;
            this.precio = precio;

        }
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
