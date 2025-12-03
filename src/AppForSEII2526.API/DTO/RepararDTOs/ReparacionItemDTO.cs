namespace AppForSEII2526.API.DTO.RepararDTOs
{
    public class ReparacionItemDTO
    {
        public ReparacionItemDTO()
        {
        }

        public ReparacionItemDTO(int herramientaId, int cantidad, string descripcion)
        {
            HerramientaId = herramientaId;
            Cantidad = cantidad;
            Descripcion = descripcion;
        }

        public ReparacionItemDTO(string nombreHerramienta, float precio, int cantidad, string descripcion)
        {
            NombreHerramienta = nombreHerramienta;
            Precio = precio;
            Cantidad = cantidad;
            Descripcion = descripcion;
        }
        public ReparacionItemDTO(int herramientaId, int cantidad, string descripcion, string nombreHerramienta, float precio)
        {
            HerramientaId = herramientaId;
            Cantidad = cantidad;
            Descripcion = descripcion;
            NombreHerramienta = nombreHerramienta;
            Precio = precio;
        }



        public string NombreHerramienta { get; set; }
        public float Precio { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Cantidad { get; set; }

        public string? Descripcion { get; set; }

        [Required, Range(1, int.MaxValue, ErrorMessage = "La herramienta es obligatoria.")]
        public int HerramientaId { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReparacionItemDTO dTO &&
                   Cantidad == dTO.Cantidad &&
                   Descripcion == dTO.Descripcion &&
                   HerramientaId == dTO.HerramientaId;
        }
    }

}
