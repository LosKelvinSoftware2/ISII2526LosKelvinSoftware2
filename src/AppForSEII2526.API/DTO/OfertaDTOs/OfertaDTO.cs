using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AppForSEII2526.API.DTO.OfertaDTOs
{
    public class OfertaDTO
    {
        public OfertaDTO(DateTime fechaFinal, DateTime fechaInicio,
            tiposMetodoPago? metodoPago, tiposDiridaOferta? dirigidaA, List<OfertaItemDTO> ofertaItems)
        {
            this.fechaFinal = fechaFinal;
            this.fechaInicio = fechaInicio;
            this.fechaOferta = DateTime.Today;
            this.ofertaItems = ofertaItems;
            this.metodoPago = metodoPago; 
            this.dirigidaA = dirigidaA;
        }

        public override bool Equals(object? obj)
        {
            return obj is OfertaDTO dTO &&
                   // Comparar los DateTime por Ticks para evitar diferencias de Kind/offset
                   fechaFinal.Ticks == dTO.fechaFinal.Ticks &&
                   fechaInicio.Ticks == dTO.fechaInicio.Ticks &&
                   fechaOferta.Ticks == dTO.fechaOferta.Ticks &&
                   ofertaItems.SequenceEqual(dTO.ofertaItems) &&
                   metodoPago == dTO.metodoPago &&
                   dirigidaA == dTO.dirigidaA;
        }

        [Required(ErrorMessage = "Por favor, introduce la fecha final")]
        public DateTime fechaFinal { get; set; }

        [Required(ErrorMessage = "Por favor, introduce la fecha de inicio")]
        public DateTime fechaInicio { get; set; } // No puede ser anterior a hoy
        public DateTime fechaOferta { get; set; } // Debe ser después de la fecha de inicio y antes de la fecha final

        // Conexiones otras tablas
        public List<OfertaItemDTO> ofertaItems { get; set; }

        [Required(ErrorMessage = "Por favor, elige el método de pago")]
        public tiposMetodoPago? metodoPago { get; set; }

        // La interrogación significa que puede ser nulo
        public tiposDiridaOferta? dirigidaA { get; set; }
    }
}

