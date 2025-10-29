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
        [Route("{id}")]
        [ProducesResponseType(typeof(ReparacionDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetReparacion(int id)
        {
            if (_context.Reparacion == null)
            {
                _logger.LogError("Error: Reparaciones table does not exist");
                return NotFound();
            }

            var reparacion = await _context.Reparacion
                .Where(r => r.Id == id)
                .Include(r => r.ItemsReparacion)         // join con ReparacionItem
                    .ThenInclude(ri => ri.Herramienta)   // join con Herramienta
                .Select(r => new ReparacionDTO(
                    r.Id,
                    r.Cliente,                           // Nombre completo del cliente
                    r.FechaEntrega,
                    r.FechaRecogida,
                    r.PrecioTotal,
                    r.ItemsReparacion
                        .Select(ri => new ReparacionItemDTO(
                            ri.Cantidad,
                            ri.Descripcion,
                            ri.Herramienta.Precio,
                            ri.Herramienta.Id,
                            ri.Herramienta,
                            ri.reparacionId,
                            ri.Reparacion

                        )).ToList()
                    , r.MetodoPago
                ))
                .FirstOrDefaultAsync();

            if (reparacion == null)
            {
                _logger.LogWarning($"No se encontró la reparación con id {id}");
                return NotFound();
            }

            return Ok(reparacion);
        }

        // apartado 5 crear una nueva solicitud de reparación
        [HttpPost("Registrar")]
        [ProducesResponseType(typeof(ReparacionDetailsDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReparacionDetailsDTO>> RegistrarReparacion([FromBody] ReparacionDetailsDTO nuevaReparacion)
        {
            if (nuevaReparacion == null)
                return BadRequest("Los datos de la reparación son requeridos.");

            if (nuevaReparacion.ItemsReparacion == null || !nuevaReparacion.ItemsReparacion.Any())
                return BadRequest("Debe seleccionar al menos una herramienta para reparar.");

            // Validar fechas
            if (nuevaReparacion.FechaEntrega < DateTime.Today)
                return BadRequest("La fecha de entrega no puede ser anterior al día actual.");

            // Calcular el precio total si no viene ya calculado
            nuevaReparacion.PrecioTotal = nuevaReparacion.ItemsReparacion.Sum(i => i.Precio * i.Cantidad);

            // Crear entidad de reparación
            var reparacionEntity = new Reparacion
            {
                Cliente = await _context.Users.FirstOrDefaultAsync(u => u.Nombre == nuevaReparacion.NombreCliente && u.Apellido == nuevaReparacion.ApellidosCliente),
                FechaEntrega = nuevaReparacion.FechaEntrega,
                FechaRecogida = nuevaReparacion.FechaRecogida,
                PrecioTotal = nuevaReparacion.PrecioTotal,
                MetodoPago = nuevaReparacion.MetodoPago,
                ItemsReparacion = new List<ReparacionItem>()
            };

            // Agregar los ítems de reparación
            foreach (var item in nuevaReparacion.ItemsReparacion ?? new List<ReparacionItemDTO>())
            {
                var herramienta = await _context.Herramienta.FindAsync(item.herramientaId);
                if (herramienta == null)
                    return BadRequest($"La herramienta con ID {item.herramientaId} no existe.");

                reparacionEntity.ItemsReparacion.Add(new ReparacionItem
                {
                    Cantidad = item.Cantidad,
                    Descripcion = item.Descripcion,
                    Precio = herramienta.Precio,
                    Herramienta = herramienta
                });
            }

            _context.Reparacion.Add(reparacionEntity);
            await _context.SaveChangesAsync();

            // Crear DTO de respuesta
            var reparacionDTO = new ReparacionDetailsDTO(
                reparacionEntity.Id,
                nuevaReparacion.NombreCliente,
                nuevaReparacion.ApellidosCliente,
                reparacionEntity.FechaEntrega,
                reparacionEntity.FechaRecogida,
                reparacionEntity.PrecioTotal,
                reparacionEntity.MetodoPago,
                nuevaReparacion.ItemsReparacion
            );

            return CreatedAtAction(nameof(GetReparacion), new { id = reparacionEntity.Id }, reparacionDTO);
        }


    }
}
