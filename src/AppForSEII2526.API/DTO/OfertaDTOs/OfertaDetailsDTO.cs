



namespace AppForSEII2526.API.DTO.OfertaDTOs
{
    public class OfertaDetailsDTO: OfertaDTO
    {
        public OfertaDetailsDTO(DateTime fechaInicio, DateTime fechaFinal, DateTime fechaOferta,
            tiposMetodoPago? metodoPago, tiposDiridaOferta? dirigidaA, List<OfertaItemDTO> ofertaItems):
            base(fechaFinal, fechaInicio, metodoPago, dirigidaA, ofertaItems)
        {
            this.fechaInicio = fechaInicio;
            this.fechaFinal = fechaFinal;
            this.fechaOferta = fechaOferta;
            this.metodoPago = metodoPago;
            this.dirigidaA = dirigidaA;
            this.ofertaItems = ofertaItems;
        }

        public override bool Equals(object? obj)
        {
            return obj is OfertaDetailsDTO dTO &&
                   base.Equals(obj) &&
                   fechaFinal == dTO.fechaFinal &&
                   fechaInicio == dTO.fechaInicio &&
                   fechaOferta == dTO.fechaOferta &&
                   ofertaItems.SequenceEqual(dTO.ofertaItems) &&
                   metodoPago == dTO.metodoPago &&
                   dirigidaA == dTO.dirigidaA;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(fechaInicio, fechaFinal, fechaOferta, metodoPago, dirigidaA, ofertaItems);
        }
    }
}
