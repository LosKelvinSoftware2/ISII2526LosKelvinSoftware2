using AppForSEII2526.API.DTO.RepararDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReparacionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompraController> _logger;

        public ReparacionController(ApplicationDbContext context, ILogger<CompraController> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        // GET details apartado 7
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<ReparacionDetailsDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetReparacionDetails()
        {
            if (_context.Reparacion == null)
            {
                _logger.LogError("Error: La tabla Reparacion no existe");
                return NotFound();
            }

            var reparaciones = await _context.Reparacion
                .Include(r => r.Cliente)
                .Include(r => r.ItemsReparacion)
                    .ThenInclude(ri => ri.Herramienta)
                .Select(r => new ReparacionDetailsDTO(
                    r.Id,
                    r.Cliente.Nombre,
                    r.Cliente.Apellido,
                    r.Cliente.PhoneNumber,
                    r.FechaEntrega,
                    r.FechaRecogida,
                    r.PrecioTotal,
                    r.MetodoPago,
                    r.ItemsReparacion
                        .Select(ri => new ReparacionItemDetailsDTO(
                            ri.Herramienta.Nombre,
                            ri.Precio,
                            ri.Cantidad,
                            ri.Descripcion
                            
                        )).ToList()
                    
                ))
                .ToListAsync();

            if (reparaciones == null || reparaciones.Count == 0)
            {
                _logger.LogWarning("No se encontraron reparaciones registradas.");
                return NotFound();
            }

            return Ok(reparaciones);
        }

        // apartado 5 crear una nueva solicitud de reparación
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReparacionDetailsDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreateReparacion(ReparacionDTO reparacionForCreate)
        {
            // Buscar usuario por UserName (que es el email según tu BD)
            var user = _context.ApplicationUser.FirstOrDefault(au => au.UserName == reparacionForCreate.UserName);
            if (user == null)
                ModelState.AddModelError("Usuario", "Error! Usuario no autenticado o no encontrado");

            // Validaciones de fechas
            if (reparacionForCreate.FechaEntrega <= DateTime.Today)
                ModelState.AddModelError("FechaEntrega", "Error! La fecha de entrega debe ser posterior a hoy");

            // Validación de items
            if (reparacionForCreate.ItemsReparacion == null || reparacionForCreate.ItemsReparacion.Count == 0)
                ModelState.AddModelError("ItemsReparacion", "Error! Debe incluir al menos una herramienta para reparar");

            // Validación de cantidades (Flujo Alternativo 5)
            if (reparacionForCreate.ItemsReparacion != null && reparacionForCreate.ItemsReparacion.Any(item => item.Cantidad <= 0))
                ModelState.AddModelError("ItemsReparacion", "Error! La cantidad de todas las herramientas debe ser al menos 1");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // Resto del código permanece igual...
            var herramientaIds = reparacionForCreate.ItemsReparacion.Select(ri => ri.HerramientaId).ToList();

            var herramientas = await _context.Herramienta
                .Include(h => h.fabricante)
                .Where(h => herramientaIds.Contains(h.Id))
                .Select(h => new {
                    h.Id,
                    h.Nombre,
                    h.Precio,
                    h.TiempoReparacion,
                    EstaEnReparacion = _context.ReparacionItem.Any(ri => ri.herramientaId == h.Id)
                })
                .ToListAsync();

            var reparacion = new Reparacion
            {
                Cliente = user,
                FechaEntrega = reparacionForCreate.FechaEntrega,
                FechaRecogida = reparacionForCreate.FechaEntrega,
                MetodoPago = reparacionForCreate.MetodoPago,
                ItemsReparacion = new List<ReparacionItem>()
            };

            float precioTotal = 0;
            int maxTiempoReparacion = 0;

            foreach (var item in reparacionForCreate.ItemsReparacion)
            {
                var herramienta = herramientas.FirstOrDefault(h => h.Id == item.HerramientaId);

                if (herramienta == null)
                {
                    ModelState.AddModelError("ItemsReparacion", $"Error! La herramienta con ID {item.HerramientaId} no existe");
                }
                else if (herramienta.EstaEnReparacion)
                {
                    ModelState.AddModelError("ItemsReparacion", $"Error! La herramienta '{herramienta.Nombre}' no está disponible para reparación");
                }
                else
                {
                    float precioItem = herramienta.Precio * item.Cantidad;
                    precioTotal += precioItem;

                    if (herramienta.TiempoReparacion > maxTiempoReparacion)
                        maxTiempoReparacion = herramienta.TiempoReparacion;

                    var reparacionItem = new ReparacionItem
                    {
                        herramientaId = item.HerramientaId,
                        Cantidad = item.Cantidad,
                        Descripcion = item.Descripcion,
                        Precio = precioItem,
                        Herramienta = await _context.Herramienta.FindAsync(item.HerramientaId)
                    };

                    reparacion.ItemsReparacion.Add(reparacionItem);
                }
            }

            reparacion.FechaRecogida = CalcularFechaRecogida(reparacion.FechaEntrega, maxTiempoReparacion);
            reparacion.PrecioTotal = precioTotal;

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            _context.Reparacion.Add(reparacion);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Reparacion", $"Error! Hubo un problema al guardar la reparación, por favor intente más tarde");
                return Conflict("Error: " + ex.Message);
            }

            var itemsDetails = reparacion.ItemsReparacion.Select(ri => new ReparacionItemDetailsDTO(
                ri.Herramienta.Nombre,
                ri.Precio,
                ri.Cantidad,
                ri.Descripcion
            )).ToList();

            var reparacionDetail = new ReparacionDetailsDTO(
                reparacion.Id,
                user.Nombre,
                user.Apellido,
                user.PhoneNumber,
                reparacion.FechaEntrega,
                reparacion.FechaRecogida,
                reparacion.PrecioTotal,
                reparacion.MetodoPago,
                itemsDetails
            );

            return CreatedAtAction("GetReparacionDetails", new { id = reparacion.Id }, reparacionDetail);
        }

        private DateTime CalcularFechaRecogida(DateTime fechaEntrega, int diasHabiles)
        {
            DateTime fechaRecogida = fechaEntrega;
            int diasAgregados = 0;

            while (diasAgregados < diasHabiles)
            {
                fechaRecogida = fechaRecogida.AddDays(1);
                if (fechaRecogida.DayOfWeek != DayOfWeek.Saturday && fechaRecogida.DayOfWeek != DayOfWeek.Sunday)
                {
                    diasAgregados++;
                }
            }

            return fechaRecogida;
        }



    }
}
