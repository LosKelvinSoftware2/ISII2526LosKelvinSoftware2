namespace AppForSEII2526.API.DTO.OfertaDTOs
{
    public class OfertaDetailsDTO: OfertaDTO
    {
        public OfertaDetailsDTO(int id, string fechaInicio, string fechaFinal, string fechaOferta,
            tiposMetodoPago metodoPago, tiposDiridaOferta? dirigidaA, IList<OfertaItemDTO> ofertaItems):
            base(id, fechaFinal, fechaInicio, fechaOferta, metodoPago, dirigidaA, ofertaItems)
        {
            this.id = id;
            this.fechaInicio = fechaInicio;
            this.fechaFinal = fechaFinal;
            this.fechaOferta = fechaOferta;
            this.metodoPago = metodoPago;
            this.dirigidaA = dirigidaA;
            this.ofertaItems = ofertaItems;
        }

        public int id { get; set; }

        public string fechaInicio { get; set; }

        public string fechaFinal { get; set; }
        public string fechaOferta { get; set; }

        public tiposMetodoPago metodoPago { get; set; }

        public tiposDiridaOferta? dirigidaA { get; set; }

        public IList<OfertaItemDTO> ofertaItems { get; set; }
    }
}
