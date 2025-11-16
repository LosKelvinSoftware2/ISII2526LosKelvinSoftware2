namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(herramientaId), nameof(alquilerId))]
    public class AlquilarItem

    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int cantidad { get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        public float precio { get; set; }
        //FK hacia Alquiler
        public int alquilerId { get; set; }
        public Alquiler alquiler { get; set; }

        //FK hacia Herramienta
        public int herramientaId { get; set; }
        public Herramienta herramienta { get; set; }


    }
}