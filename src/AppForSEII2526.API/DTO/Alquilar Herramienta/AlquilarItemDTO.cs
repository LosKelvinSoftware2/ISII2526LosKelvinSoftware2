namespace AppForSEII2526.API.DTO.Alquilar_Herramienta
{
    public class AlquilarItemDTO
    {
        public AlquilarItemDTO(Herramienta herramienta , int cantidad , float precio)
        {
            nombreHerramienta = herramienta.Nombre;
            materialHerramienta = herramienta.Material;
            this.cantidad = cantidad;
            this.precio = precio;

        }
        [Required]
        public string nombreHerramienta { get; set; }
        [Required]
        public string materialHerramienta { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int cantidad { get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        public float precio { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AlquilarItemDTO dTO &&
                   nombreHerramienta == dTO.nombreHerramienta &&
                   materialHerramienta == dTO.materialHerramienta &&
                   cantidad == dTO.cantidad &&
                   precio == dTO.precio;
        }
    }
}
