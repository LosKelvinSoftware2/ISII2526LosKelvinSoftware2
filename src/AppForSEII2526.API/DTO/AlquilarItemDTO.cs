namespace AppForSEII2526.API.DTO
{
    public class AlquilarItemDTO
    {
        public AlquilarItemDTO(int cantidad, float precio, int alquilerId, Alquiler alquiler, int herramientaId, Herramienta herramienta)
        {
            this.cantidad = cantidad;
            this.precio = precio;
            this.alquilerId = alquilerId;
            this.alquiler = alquiler;
            this.herramientaId = herramientaId;
            this.herramienta = herramienta;
        }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int cantidad { get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        public float precio { get; set; }
        //FK hacia Alquiler
        public int alquilerId { get; set; }
        public Alquiler alquiler { get; set; }

        //FK hacia Herramienta
        public int herramientaId { get; set; }
        public Herramienta herramienta { get; set; }
    }
}
