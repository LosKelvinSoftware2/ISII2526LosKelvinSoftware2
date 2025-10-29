namespace AppForSEII2526.API.DTO.OfertaDTOs
{
    public class OfertaDetailsDTO: OfertaDTO
    {
        public OfertaDetailsDTO(int id, float porcentaje, DateTime fechaInicio, DateTime fechaFinal, DateTime fechaOferta,
            tiposMetodoPago metodoPago, tiposDiridaOferta? dirigidaA, IList<OfertaItemDTO> ofertaItems):
            base(id, porcentaje, fechaFinal, fechaInicio, metodoPago, dirigidaA, ofertaItems)
        {
            this.id = id;
            this.porcentaje = porcentaje;
            this.fechaInicio = fechaInicio;
            this.fechaFinal = fechaFinal;
            this.fechaOferta = fechaOferta;
            this.metodoPago = metodoPago;
            this.dirigidaA = dirigidaA;
            this.ofertaItems = ofertaItems;
        }

        public int id { get; set; }

        public float porcentaje { get; set; }

        public DateTime fechaInicio { get; set; }

        public DateTime fechaFinal { get; set; }
        public DateTime fechaOferta { get; set; }

        public tiposMetodoPago metodoPago { get; set; }

        public tiposDiridaOferta? dirigidaA { get; set; }

        public IList<OfertaItemDTO> ofertaItems { get; set; }
    }
}
