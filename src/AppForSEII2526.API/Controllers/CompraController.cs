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
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompraController> _logger;

        public CompraController(ApplicationDbContext context, ILogger<CompraController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Compra/{id}
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompraDetailsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCompraDetails(int id)
        {
            if (_context.Compra == null)
            {
                _logger.LogError("Error: Compra table does not exist");
                return NotFound();
            }

            var compra = await _context.Compra
                .Where(c => c.Id == id)
                .Include(c => c.CompraItems)
                    .ThenInclude(ci => ci.herramienta)
                .Select(c => new CompraDetailsDTO(
                    c.Id,
                    c.fechaCompra,
                    c.Cliente.Nombre,
                    c.Cliente.Apellido,
                    c.Cliente.telefono,
                    c.Cliente.correoelectronico,
                    c.direccionEnvio,
                    c.PrecioTotal,
                    c.CompraItems.Select(ci => new CompraItemDTO(
                        ci.herramienta.Id,
                        ci.herramienta.Nombre,
                        ci.herramienta.Material,
                        ci.cantidad,
                        ci.precio
                    )).ToList(),
                    c.MetodoPago
                ))
                .FirstOrDefaultAsync();

            if (compra == null)
            {
                _logger.LogWarning($"Compra con id {id} no encontrada.");
                return NotFound();
            }

            return Ok(compra);
        }

        // POST: api/Compra/CrearCompra
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompraDetailsDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> CreateCompra(CompraDTO dto)
        {
            // Validaciones básicas
            if (dto.fechaCompra < DateTime.Today)
                ModelState.AddModelError("FechaCompra", "La compra no puede realizarse en una fecha anterior a hoy");

            if (dto.CompraItems == null || dto.CompraItems.Count == 0)
                ModelState.AddModelError("CompraItems", "Debe haber al menos una herramienta para comprar");

            if (string.IsNullOrEmpty(dto.nombreCliente))
                ModelState.AddModelError("Cliente.Nombre", "El nombre es obligatorio");
            if (string.IsNullOrEmpty(dto.apellidoCliente))
                ModelState.AddModelError("Cliente.Apellido", "El apellido es obligatorio");
            if (string.IsNullOrEmpty(dto.direccionEnvio))
                ModelState.AddModelError("direccionEnvio", "La dirección de envío es obligatoria");
            if (!Enum.IsDefined(typeof(tiposMetodoPago), dto.MetodoPago))
                ModelState.AddModelError("metodoPago", "El método de pago es obligatorio");

            // Validar cantidad de cada herramienta
            if (dto.CompraItems != null)
            {
                foreach (var item in dto.CompraItems)
                {
                    if (item.cantidad <= 0)
                        ModelState.AddModelError("Cantidad", "Debe especificarse una cantidad válida para cada herramienta");
                }
            }

            // EXAMEN Descripciones nulas: cantidad = 3 y descripción vacía
            if (dto.CompraItems != null)
            {
                foreach (var item in dto.CompraItems)
                {
                    if (string.IsNullOrWhiteSpace(item.descripcion) && item.cantidad == 3)
                    {
                        ModelState.AddModelError("Descripcion", "¡Error! Estás comprando demasiadas herramientas sin descripción");
                    }
                }
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            var herramientasNombre = dto.CompraItems.Select(ci => ci.nombre).ToList();
            var herramientasLista = await _context.Herramienta
                .Where(h => herramientasNombre.Contains(h.Nombre))
                .ToListAsync();

            // Crear compra
            var compra = new Compra
            {
                Cliente = new ApplicationUser
                {
                    Nombre = dto.nombreCliente,
                    Apellido = dto.apellidoCliente,
                    telefono = dto.telefonoCliente,
                    correoelectronico = dto.correoCliente,
                },
                direccionEnvio = dto.direccionEnvio,
                fechaCompra = DateTime.UtcNow,
                MetodoPago = dto.MetodoPago,
                CompraItems = new List<CompraItem>()
            };

            compra.PrecioTotal = 0;

            foreach (var item in dto.CompraItems)
            {
                var herramienta = herramientasLista.FirstOrDefault(h => h.Nombre == item.nombre);
                if (herramienta == null) continue;

                compra.CompraItems.Add(new CompraItem
                {
                    cantidad = item.cantidad,
                    precio = herramienta.Precio,
                    herramientaId = herramienta.Id,
                    //Ponemos unan descripción por defecto si no se proporciona una (ya que está definida como obligatoria)
                    descripcion = item.descripcion
                });

                compra.PrecioTotal += herramienta.Precio * item.cantidad;
            }

            _context.Compra.Add(compra);

                await _context.SaveChangesAsync();
            

            var compraDTO = new CompraDetailsDTO(
                compra.Id,
                compra.fechaCompra,
                compra.Cliente.Nombre,
                compra.Cliente.Apellido,
                compra.Cliente.telefono,
                compra.Cliente.correoelectronico,
                compra.direccionEnvio,
                compra.PrecioTotal,
                compra.CompraItems.Select(ci => new CompraItemDTO(
                    ci.herramienta.Id,
                    ci.herramienta.Nombre,
                    ci.herramienta.Material,
                    ci.cantidad,
                    ci.precio
                )).ToList(),
                compra.MetodoPago
            );

            return CreatedAtAction(nameof(GetCompraDetails), new { id = compra.Id }, compraDTO);
        }
    }
}
