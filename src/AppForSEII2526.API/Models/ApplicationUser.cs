using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; }

    [Required]
    [MaxLength(100)]
    public string Apellido { get; set; }

    // Opcional
    [Phone]
    public double telefono { get; set; }

    [EmailAddress]
    public string correoelectronico { get; set; }

    // Relación con Reparaciones (uno a muchos)
    public List<Reparacion> Reparaciones { get; set; } = new();

    // Relación con Compras (uno a muchos)
    public List<Compra> Compras { get; set; } = new();

    // Relación con Compras (uno a muchos)
    public List<Alquiler> Alquileres { get; set; } = new();
}