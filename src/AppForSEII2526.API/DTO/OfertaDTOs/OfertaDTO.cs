namespace AppForSEII2526.API.DTO.OfertaDTOs
{
    public class OfertaDTO
    {

        public OfertaDTO(float porcentaje, DateTime fechaFinal, DateTime fechaInicio,
            tiposMetodoPago metodoPago, tiposDiridaOferta? dirigidaA, IList<OfertaItemDTO> ofertaItems)
        {
            this.porcentaje = porcentaje;
            this.fechaFinal = fechaFinal;
            this.fechaInicio = fechaInicio;
            this.fechaOferta = DateTime.Today;
            this.ofertaItems = ofertaItems;
            this.metodoPago = metodoPago;
            this.dirigidaA = dirigidaA;
        }


        [Required]
        public DateTime fechaFinal { get; set; }

        [Required]
        [Range(0.0, 100.0)]
        public float porcentaje { get; set; }

        [Required]
        public DateTime fechaInicio { get; set; } // No puede ser anterior a hoy
        public DateTime fechaOferta { get; set; } // Debe ser después de la fecha de inicio y antes de la fecha final

        // Conexiones otras tablas
        public IList<OfertaItemDTO> ofertaItems { get; set; }

        [Required]
        public tiposMetodoPago metodoPago { get; set; }

        // La interrogación significa que puede ser nulo
        public tiposDiridaOferta? dirigidaA { get; set; }
    }
}

