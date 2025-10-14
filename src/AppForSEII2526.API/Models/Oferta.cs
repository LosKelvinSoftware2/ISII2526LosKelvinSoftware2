using System;
using System.Runtime.InteropServices;

namespace AppForSEII2526.API.Models
{
    public class Oferta
    {
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
