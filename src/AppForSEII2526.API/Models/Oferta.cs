using System;

namespace AppForSEII2526.API.Models
{
    public class Oferta
    {
        public int Id { get; set; }
        public String fechaFinal { get; set; }
        public String fechaInicio { get; set; }
        public String fechaOferta { get; set; }

        // Conexiones otras tablas
        public List<OfertaItem> ofertaItems { get; set; }
        public tiposMetodoPago metodoPago { get; set; }
        public tiposDiridaOferta dirigidaA { get; set; }
    }
}
