using Microsoft.AspNetCore.Http.HttpResults;

namespace AppForSEII2526.API.DTO.OfertaDTOs
{
    public class OfertaItemDTO
    {
        
        public OfertaItemDTO(float? porcentaje, float precioFinal, string nombre, string material,
            string Fabricante, float precioOriginal)
        {
            this.porcentaje = porcentaje;
            this.precioFinal = precioFinal;
            this.nombre = nombre;
            this.material = material;
            this.Fabricante = Fabricante;
            this.precioOriginal = precioOriginal; // Viene de Herramienta.
        }

        public override bool Equals(object? obj)
        {
            return obj is OfertaItemDTO dTO &&
                   precioFinal == dTO.precioFinal &&
                   Fabricante == dTO.Fabricante &&
                   precioOriginal == dTO.precioOriginal &&
                   nombre == dTO.nombre &&
                   material == dTO.material &&
                   porcentaje == dTO.porcentaje;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(precioFinal, Fabricante, precioOriginal, nombre, material, porcentaje);
        }


        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "El precio total no puede ser negativo.")]
        public float precioFinal { get; set; }

        public string Fabricante { get; set; }

        public float precioOriginal { get; set; }

        public string nombre { get; set; }

        public string material { get; set; }

        [Required(ErrorMessage = "Por favor, introduce el porcentaje de descuento")]
        [Range(1, 100.0, ErrorMessage = "El porcentaje debe estar entre 1 y 100.")]
        public float? porcentaje { get; set; }
    }
}
