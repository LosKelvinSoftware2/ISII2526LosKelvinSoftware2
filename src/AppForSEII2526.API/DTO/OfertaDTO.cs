namespace AppForSEII2526.API.DTO
{
    public class OfertaDTO
    {
        private object value;
        private Func<List<OfertaItemDTO>> toList;

        public OfertaDTO(int id, string fechaFinal, string fechaInicio, string fechaOferta, Func<List<OfertaItemDTO>> toList, tiposMetodoPago metodoPago, tiposDiridaOferta? dirigidaA)
        {
            Id = id;
            this.fechaFinal = fechaFinal;
            this.fechaInicio = fechaInicio;
            this.fechaOferta = fechaOferta;
            this.toList = toList;
            this.metodoPago = metodoPago;
            this.dirigidaA = dirigidaA;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public String fechaFinal { get; set; }

        [Required]
        public String fechaInicio { get; set; } // No puede ser anterior a hoy
        public String fechaOferta { get; set; } // Debe ser después de la fecha de inicio y antes de la fecha final

        // Conexiones otras tablas
        public List<OfertaItem> ofertaItems { get; set; }

        [Required]
        public tiposMetodoPago metodoPago { get; set; }

        // La interrogación significa que puede ser nulo
        public tiposDiridaOferta? dirigidaA { get; set; }
    }
}

