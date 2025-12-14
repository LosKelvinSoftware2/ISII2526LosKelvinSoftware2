using AppForSEII2526.API.DTO.RepararDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReparacionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReparacionController> _logger;

        public ReparacionController(ApplicationDbContext context, ILogger<ReparacionController> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        // GET details apartado 7
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<ReparacionDetailsDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetReparacionDetails(int id)
        {
            if (_context.Reparacion == null)
            {
                _logger.LogError("Error: La tabla Reparacion no existe");
                return NotFound();
            }

            var reparaciones = await _context.Reparacion
                .Where(r => r.Id == id)
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
                        .Select(ri => new ReparacionItemDTO(
                            ri.Herramienta.Nombre,
                            ri.Precio,
                            ri.Cantidad,
                            ri.Descripcion
                            
                        )).ToList()
                    
                ))
                .ToListAsync();

            if (reparaciones == null)
            {
                _logger.LogWarning($"Reparación no encontrada.");
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
            // === Validación de reglas de negocio (Flujos Alternativos) ===

            // Flujo Alternativo 1: fecha de entrega <= hoy ??
            if (reparacionForCreate.FechaEntrega.Date < DateTime.Today)
            {
                ModelState.AddModelError("fechaEntrega", "La fecha de entrega debe ser igual o posterior a hoy.");
            }

            // Flujo Alternativo 3 y 5: al menos una herramienta y cantidad >= 1 (cantidad ya validada en el DTO)
            if (reparacionForCreate.ItemsReparacion == null || reparacionForCreate.ItemsReparacion.Count == 0)
            {
                ModelState.AddModelError("Herramientas", "Debe incluir al menos una herramienta para reparar.");
            }
            else
            {
                // Validación extra (ya validad en el DTO): cantidad > 0
                foreach (var item in reparacionForCreate.ItemsReparacion)
                {
                    if (item.Cantidad <= 0)
                    {
                        ModelState.AddModelError("Herramientas", $"La cantidad de la herramienta " +
                            $"'{item.NombreHerramienta}' debe ser mayor que 0.");
                    }
                }
            }

            // Validación extra para el enum metodoPago
            if (!Enum.IsDefined(typeof(tiposMetodoPago), reparacionForCreate.MetodoPago))
            {
                ModelState.AddModelError("metodoPago",
                    "El método de pago no es válido. Valores permitidos: 0 (Efectivo), 1 (TarjetaCredito), 2 (PayPal).");
            }

            // EXAMEN DE PRÁCTICAS DEL SPRINT 2: REVISAR NUMERO DE TELEFONO
            if (!string.IsNullOrWhiteSpace(reparacionForCreate.NumTelefono) &&
               !reparacionForCreate.NumTelefono.StartsWith("+34")) // Comprobamos que empiece por +34
            {
                ModelState.AddModelError("NumTelefono", "Error!, el teléfono debe empezar por +34"); // Devolver Bad Request
            }

            // cliente en la base de datos (AspNetUsers) ??
            var cliente = await _context.ApplicationUser
                .FirstOrDefaultAsync(u => u.Nombre == reparacionForCreate.NombreCliente &&
                u.Apellido == reparacionForCreate.ApellidosCliente); // por nombre y apellidos

            if (cliente == null)
            {
                ModelState.AddModelError("Cliente", "El cliente no está registrado en el sistema.");
            }
            else
            {
                //actualizar el numero del cliente para el DETAILS
                if (!string.IsNullOrEmpty(reparacionForCreate.NumTelefono) &&
                cliente.PhoneNumber != reparacionForCreate.NumTelefono)
                {
                    cliente.PhoneNumber = reparacionForCreate.NumTelefono;
                    // Marcamos que el objeto 'cliente' ha sido modificado para que EF Core lo guarde
                    _context.Entry(cliente).State = EntityState.Modified;
                }
            }

            // Validación de herramientas
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

            // Si hay errores de validación, devolver Bad Request
            if (ModelState.ErrorCount > 0) // comprobar errores acumulados
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var reparacion = new Reparacion
            {
                Cliente = cliente,
                FechaEntrega = reparacionForCreate.FechaEntrega,
                FechaRecogida = reparacionForCreate.FechaEntrega,
                MetodoPago = reparacionForCreate.MetodoPago,
                ItemsReparacion = new List<ReparacionItem>()
            };

            // === Cálculo de la fecha de recogida ===
            float precioTotal = 0;
            int maxTiempoReparacion = 0;

            foreach (var item in reparacionForCreate.ItemsReparacion)
            {
                var herramienta = herramientas.FirstOrDefault(h => h.Id == item.HerramientaId);

                if (herramienta == null)
                {
                    ModelState.AddModelError("ItemsReparacion", $"Error! La herramienta con ese ID no existe");
                }
                else if (herramienta.EstaEnReparacion)
                {
                    ModelState.AddModelError("ItemsReparacion", $"Error! La herramienta con ese nombre no está disponible para reparación");
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

            var itemsDetails = reparacion.ItemsReparacion.Select(ri => new ReparacionItemDTO(
                ri.Herramienta.Nombre,
                ri.Precio,
                ri.Cantidad,
                ri.Descripcion
                
            )).ToList();

            var reparacionDetail = new ReparacionDetailsDTO(
                reparacion.Id,
                cliente.Nombre,
                cliente.Apellido,
                cliente.PhoneNumber,
                reparacion.FechaEntrega,
                reparacion.FechaRecogida,
                reparacion.PrecioTotal,
                reparacion.MetodoPago,
                itemsDetails
            );

            return CreatedAtAction(nameof(GetReparacionDetails), new { id = reparacion.Id }, reparacionDetail);
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
