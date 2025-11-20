namespace AppForSEII2526.API.Models
{
    public class Compra
    {
        [Key]
        public int Id { get; set; }

        //Vinculación con el usuario (cliente autenticado)
        [Required]
        public ApplicationUser Cliente { get; set; }

        [Required]
        [MaxLength(200)]
        public string direccionEnvio { get; set; }

        [Required]
        public DateTime fechaCompra { get; set; } 

        public string descripcion { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "El precio total no puede ser negativo.")]
        public float PrecioTotal { get; set; }


        public List<CompraItem> CompraItems { get; set; } = new();


        [Required]
        public tiposMetodoPago MetodoPago { get; set; }

        
    }
}
