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
        [ProducesResponseType(typeof(List<ReparacionDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetReparacion()
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
                .Select(r => new ReparacionDTO(
                    r.Id,
                    r.Cliente.Nombre,
                    r.Cliente.Apellido,
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
                        )).ToList(),
                    r.MetodoPago
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
        [HttpPost("Registrar")]
        [ProducesResponseType(typeof(ReparacionDetailsDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReparacionDetailsDTO>> RegistrarReparacion([FromBody] ReparacionDetailsDTO nuevaReparacion)
        {
            if (nuevaReparacion == null)
                return BadRequest("Los datos de la reparación son requeridos.");

            if (string.IsNullOrWhiteSpace(nuevaReparacion.NombreCliente) ||
                string.IsNullOrWhiteSpace(nuevaReparacion.ApellidosCliente))
                return BadRequest("El nombre y los apellidos del cliente son obligatorios.");

            if (nuevaReparacion.ItemsReparacion == null || !nuevaReparacion.ItemsReparacion.Any())
                return BadRequest("Debe seleccionar al menos una herramienta para reparar.");

            if (nuevaReparacion.FechaEntrega < DateTime.Today)
                return BadRequest("La fecha de entrega no puede ser anterior al día actual.");

            if (!Enum.IsDefined(typeof(tiposMetodoPago), nuevaReparacion.MetodoPago))
                return BadRequest("El método de pago es inválido.");

            // Buscar cliente por nombre y apellidos
            var cliente = await _context.Users
                .FirstOrDefaultAsync(u => u.Nombre == nuevaReparacion.NombreCliente && u.Apellido == nuevaReparacion.ApellidosCliente);

            if (cliente == null)
                return BadRequest("El cliente no existe. Debe estar registrado en el sistema.");

            // Calcular precio total basado en las herramientas
            float precioTotal = 0;
            var itemsEntidad = new List<ReparacionItem>();

            foreach (var itemDto in nuevaReparacion.ItemsReparacion)
            {
                var herramienta = await _context.Herramienta.FindAsync(itemDto.herramientaId);
                if (herramienta == null)
                    return BadRequest($"La herramienta con ID {itemDto.herramientaId} no existe.");

                var precioItem = herramienta.Precio * itemDto.Cantidad;
                precioTotal += precioItem;

                itemsEntidad.Add(new ReparacionItem
                {
                    Cantidad = itemDto.Cantidad,
                    Descripcion = itemDto.Descripcion,
                    Precio = herramienta.Precio,
                    Herramienta = herramienta
                });
            }

            // Crear la entidad de reparación
            var reparacionEntity = new Reparacion
            {
                Cliente = cliente,
                FechaEntrega = nuevaReparacion.FechaEntrega,
                FechaRecogida = nuevaReparacion.FechaRecogida,
                PrecioTotal = precioTotal,
                MetodoPago = nuevaReparacion.MetodoPago,
                ItemsReparacion = itemsEntidad
            };

            _context.Reparacion.Add(reparacionEntity);
            await _context.SaveChangesAsync();

            // Construir DTO de respuesta
            var reparacionDTO = new ReparacionDetailsDTO(
                reparacionEntity.Id,
                cliente.Nombre,
                cliente.Apellido,
                reparacionEntity.FechaEntrega,
                reparacionEntity.FechaRecogida,
                reparacionEntity.PrecioTotal,
                reparacionEntity.MetodoPago,
                reparacionEntity.ItemsReparacion.Select(ri => new ReparacionItemDTO(
                    ri.Cantidad,
                    ri.Descripcion,
                    ri.Precio,
                    ri.Herramienta.Id,
                    ri.Herramienta,
                    ri.reparacionId,
                    ri.Reparacion
                )).ToList()
            );

            return CreatedAtAction(nameof(GetReparacion), new { id = reparacionEntity.Id }, reparacionDTO);
        }



    }
}
