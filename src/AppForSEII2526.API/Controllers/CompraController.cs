using AppForSEII2526.API.DTO.Comprar_Herramienta;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<CompraController> logger;

        public CompraController(ApplicationDbContext context, ILogger<CompraController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        // GET: api/Compra/{id}

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(CompraDetailsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CompraDetailsDTO>> GetCompra(int id)
        {
            if (context.Compra == null)
            {
                logger.LogError("Error: Compras table does not exist");
                return NotFound();
            }

            var compra = await context.Compra
                .Where(c => c.Id == id)
                .Include(c => c.CompraItems)
                    .ThenInclude(ci => ci.herramienta)
                .Select(c => new CompraDetailsDTO(
                    c.Id,
                    c.Cliente,
                    c.direccionEnvio,
                    c.fechaCompra,
                    c.PrecioTotal,
                    c.CompraItems.Select(ci => new CompraItemDTO(
                        ci.cantidad,
                        ci.descripcion,
                        ci.precio,
                        ci.herramientaId,
                        ci.herramienta,
                        ci.compraId,
                        ci.compra
                    )).ToList(),
                    c.MetodoPago
                ))
                .FirstOrDefaultAsync();

            if (compra == null)
            {
                logger.LogWarning($"Compra con id {id} no encontrada.");
                return NotFound();
            }

            return Ok(compra);
        }

        // POST: api/Compra/CrearCompra

        [HttpPost]
        [Route("CrearCompra")]
        [ProducesResponseType(typeof(CompraDetailsDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CompraDetailsDTO>> CrearCompra([FromBody] CompraDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // Crear usuario temporal (o buscar existente)
            var usuario = new ApplicationUser
            {
                Nombre = dto.Cliente.Nombre,     
                Apellido = dto.Cliente.Apellido,   
                telefono = dto.Cliente.telefono,
                correoelectronico = dto.Cliente.correoelectronico,
            };

            var compra = new Compra
            {
                Cliente = usuario,
                direccionEnvio = dto.direccionEnvio,
                MetodoPago = dto.MetodoPago,
                fechaCompra = DateTime.UtcNow,
                PrecioTotal = 0,
                CompraItems = new List<CompraItem>()
            };

            foreach (var item in dto.CompraItemsDTO ?? new List<CompraItemDTO>())
            {
                var herramienta = await context.Herramienta.FindAsync(item.herramientaId);
                if (herramienta == null)
                {
                    ModelState.AddModelError("Items", $"Herramienta con ID {item.herramientaId} no encontrada");

                }

                var compraItem = new CompraItem
                {
                    herramienta = herramienta,
                    cantidad = item.cantidad,
                    descripcion = item.descripcion ?? string.Empty,
                    precio = herramienta.Precio * item.cantidad,
                    compra = compra
                };

                compra.CompraItems.Add(compraItem);
                compra.PrecioTotal += compraItem.precio;
            }

            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            context.Compra.Add(compra);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                ModelState.AddModelError("Compra", $"Error al guardar la compra: {ex.Message}");
                return Conflict("Error al guardar la compra");
            }

            // Mapear a DTO de salida
            var compraDTO = new CompraDetailsDTO(
                compra.Id,
                compra.Cliente,
                compra.direccionEnvio,
                compra.fechaCompra,
                compra.PrecioTotal,
                compra.CompraItems.Select(ci => new CompraItemDTO(
                    ci.cantidad,
                    ci.descripcion,
                    ci.precio,
                    ci.herramientaId,
                    ci.herramienta,
                    ci.compraId,
                    ci.compra
                )).ToList(),
                compra.MetodoPago
            );

            return CreatedAtAction(nameof(GetCompra), new { id = compra.Id }, compraDTO);
        }
    }

}
