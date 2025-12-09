



namespace AppForSEII2526.API.DTO.OfertaDTOs
{
    public class OfertaDetailsDTO: OfertaDTO
    {
        public OfertaDetailsDTO(int Id, DateTime fechaInicio, DateTime fechaFinal, DateTime fechaOferta,
            tiposMetodoPago? metodoPago, tiposDiridaOferta? dirigidaA, List<OfertaItemDTO> ofertaItems):
            base(Id, fechaFinal, fechaInicio, metodoPago, dirigidaA, ofertaItems)
        {
            this.Id = Id;
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
                   Id == dTO.Id &&
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
