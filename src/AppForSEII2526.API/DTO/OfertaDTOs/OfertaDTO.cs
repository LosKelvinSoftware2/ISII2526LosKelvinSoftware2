namespace AppForSEII2526.API.DTO.OfertaDTOs
{
    public class OfertaDTO
    {
        private object value;
        private Func<List<OfertaItemDTO>> toList;

        public OfertaDTO(int id, string fechaFinal, string fechaInicio, string fechaOferta, 
            tiposMetodoPago metodoPago, tiposDiridaOferta? dirigidaA, IList<OfertaItemDTO> ofertaItems)
        {
            this.Id = id;
            this.fechaFinal = fechaFinal;
            this.fechaInicio = fechaInicio;
            this.fechaOferta = fechaOferta;
            this.metodoPago = metodoPago;
            this.dirigidaA = dirigidaA;
            this.ofertaItems = ofertaItems;
        }

        [Key]
        public int Id { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduce la fecha.")]
        public string fechaFinal { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Por favor, introduce la fecha.")]
        public string fechaInicio { get; set; } // No puede ser anterior a hoy
        public string fechaOferta { get; set; } // Debe ser después de la fecha de inicio y antes de la fecha final

        // Conexiones otras tablas
        public IList<OfertaItemDTO> ofertaItems { get; set; }

        [Required]
        public tiposMetodoPago metodoPago { get; set; }

        // La interrogación significa que puede ser nulo
        public tiposDiridaOferta? dirigidaA { get; set; }
    }
}

