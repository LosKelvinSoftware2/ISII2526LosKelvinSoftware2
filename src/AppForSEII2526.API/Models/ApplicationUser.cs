using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; }

    [Required]
    [MaxLength(100)]
    public string Apellido { get; set; }
    public double Telefono { get; set; }
    public string Correo { get; set; }


    public List<Reparacion> Reparaciones { get; set; }
    public List<Alquiler> Alquileres { get; set; }
}