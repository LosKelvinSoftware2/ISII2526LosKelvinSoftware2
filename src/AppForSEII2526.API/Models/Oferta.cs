using System;
using System.Runtime.InteropServices;

namespace AppForSEII2526.API.Models
{
    public class Oferta
    {

        public Oferta()
        {
            ofertaItems = new List<OfertaItem>();
        }


        [Key]
        public int Id { get; set; }

        public float porcentaje { get; set; }

        [Required]
        public DateTime fechaFinal { get; set; }

        [Required]
        public DateTime fechaInicio { get; set; } // No puede ser anterior a hoy
        public DateTime fechaOferta { get; set; } // Debe ser después de la fecha de inicio y antes de la fecha final

        // Conexiones otras tablas
        public List<OfertaItem> ofertaItems { get; set; }

        [Required]
        public tiposMetodoPago metodoPago { get; set; }

        // La interrogación significa que puede ser nulo
        public tiposDiridaOferta? dirigidaA { get; set; }
    }
}
