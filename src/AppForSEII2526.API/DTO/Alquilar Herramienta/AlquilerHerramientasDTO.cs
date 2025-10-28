namespace AppForSEII2526.API.DTO.Alquilar_Herramienta
{
    public class AlquilerHerramientasDTO
    { 
        public AlquilerHerramientasDTO(int id, string nombre, string material, float precio, Fabricante fabricante)
        {
            Id = id;
            Nombre = nombre;
            Material = material;
            Precio = precio;
            this.fabricante = fabricante;
            AlquilerItems  = new List<AlquilarItem>();
        }
        [Key]
        public int Id { get; set; }
        [StringLength(50 , ErrorMessage = "El nombre debe tener menos de 50 caracteres.")]
        public String Nombre { get; set; }
        public String Material { get; set; }
        public float Precio { get; set; }
        //Relación con fabricante
        public Fabricante fabricante { get; set; }
        //Relación con AlquilerItem
        public List<AlquilarItem> AlquilerItems { get; set; }
    }
}
