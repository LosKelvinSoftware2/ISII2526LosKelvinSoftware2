using AppForSEII2526.API.DTO.OfertaDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertasController : ControllerBase
    {
        private readonly ApplicationDbContext _context; 
        private readonly ILogger<OfertasController> _logger;

        public OfertasController(ApplicationDbContext context, ILogger<OfertasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(OfertaDetailsDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetOferta(int id)
        {
            if (_context.Oferta == null)
            {
                _logger.LogError("Error: Oferta table does not exist");
                return NotFound();
            }

            var oferta = await _context.Oferta
                .Where (o => (o.Id == id))
                    .Include(o => o.ofertaItems)
                        .ThenInclude(oi => oi.herramienta)
                .Select(o => new OfertaDetailsDTO(
                    o.Id,
                    o.fechaInicio,
                    o.fechaFinal,
                    o.fechaOferta,
                    o.MetodoPago,
                    o.dirigidaA,
                    o.ofertaItems.Select(oi => new OfertaItemDTO(
                        oi.porcentaje,
                        oi.precioFinal,
                        oi.herramienta.Nombre,
                        oi.herramienta.Material,
                        oi.herramienta.fabricante.Nombre,
                        oi.herramienta.Precio
                    )).ToList<OfertaItemDTO>()))
                .FirstOrDefaultAsync();

            if (oferta == null)
                {
                _logger.LogError($"Error: Oferta with id { id} does not exist");
                return NotFound();
            }

            return Ok(oferta);
        }

        [HttpPost]
        [Route("[action]")]

        [ProducesResponseType(typeof(OfertaDetailsDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateOferta (OfertaDTO ofertaForCreate)
        {   
            if (ofertaForCreate.fechaInicio == DateTime.MinValue || ofertaForCreate.fechaFinal == DateTime.MinValue)
                ModelState.AddModelError("DateError", "Error! Las fechas no pueden estar vacías o en formato incorrecto.");
            if (!ofertaForCreate.metodoPago.HasValue)
                ModelState.AddModelError("MetodoPago", "Error! El método de pago es obligatorio.");
            if (ofertaForCreate.fechaInicio <= DateTime.Today)
                ModelState.AddModelError("RentalDateFrom", "Error! La fecha de inicio de oferta debe ser al menos mañana");
            if (ofertaForCreate.fechaInicio >= ofertaForCreate.fechaFinal)
                ModelState.AddModelError("RentalDateTo", "Error! La fecha final de oferta debe ser después de la fecha de inicio");
            if (ofertaForCreate.fechaFinal <= ofertaForCreate.fechaInicio.AddDays(7))
                ModelState.AddModelError("RentalDateTo", "Error!, la oferta debe durar al menos una semana");
            if (ofertaForCreate.ofertaItems.Count == 0)
                ModelState.AddModelError("RentalItems", "Error! Hay que incluir al menos una herramienta");


            // Retorno si hay errores iniciales
            if (!ModelState.IsValid)
            {
                // Flujo Alternativo 5: Datos obligatorios no rellenados (o rango incorrecto)
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // --- 2. Validación de Existencia de Herramientas ---
            var herramientaNombres = ofertaForCreate.ofertaItems.Select(oi => oi.nombre).ToList();

            // Obtener los datos de las herramientas necesarias (Nombre, Material, Fabricante, Precio Original)
            var herramientas = await _context.Herramienta
                .Where(h => herramientaNombres.Contains(h.Nombre))
                .Include(h => h.fabricante)
                .Select(h => new HerramientaForOfertaDTO(
                    h.Id,
                    h.Nombre,
                    h.Material,
                    h.Precio,
                    h.fabricante.Nombre
                )).ToListAsync();


            // --- 3. Construcción de Entidades (Modelos de DB) ---

            // Crear la entidad Oferta principal
            var nuevaOferta = new Oferta
            {
                fechaInicio = ofertaForCreate.fechaInicio,
                fechaFinal = ofertaForCreate.fechaFinal,
                fechaOferta = ofertaForCreate.fechaOferta, // La fecha de creación es ahora
                MetodoPago = ofertaForCreate.metodoPago,
                dirigidaA = ofertaForCreate.dirigidaA,
                // Las propiedades de precio/porcentaje pueden variar.
            };

            nuevaOferta.fechaOferta = DateTime.Today;
            // Crear las entidades OfertaItem y adjuntarlas a la Oferta
            foreach (var item in ofertaForCreate.ofertaItems)
            {
                if (!item.porcentaje.HasValue)
                {
                    ModelState.AddModelError("Porcentaje", $"Error! El porcentaje es obligatorio para la herramienta {item.nombre}.");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
                if (item.porcentaje < 1 || item.porcentaje > 100)
                {
                    ModelState.AddModelError("Porcentaje", "Error! El porcentaje de descuento debe estar entre 1 y 100");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }

                var herramienta = herramientas.FirstOrDefault(h => h.Nombre == item.nombre);

                if (herramienta == null)
                {
                    ModelState.AddModelError("Herramienta", $"Error! La herramienta {item.nombre} no existe en la base de datos.");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }

                // Cálculo del precio con la oferta (asumiendo que Precio en Herramienta es el original)
                float precioOriginal = herramienta.Precio;
                float porcentaje = item.porcentaje.Value / 100f; // Convertir a decimal
                float precioFinal = precioOriginal - (precioOriginal * porcentaje);

                nuevaOferta.ofertaItems.Add(new OfertaItem
                {
                    herramientaId = herramienta.Id,
                    porcentaje = item.porcentaje, // Guardamos el porcentaje en DB si es necesario
                    precioFinal = precioFinal, // El precio ya rebajado
                });
            }

            _context.Oferta.Add(nuevaOferta);

            // --- 4. Guardar en Base de Datos ---

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la nueva oferta en la base de datos.");
                return Conflict($"Error al guardar la oferta. Por favor, inténtelo de nuevo más tarde: {ex.Message}");
            }

            // --- 5. Respuesta Exitosa (Flujo Básico 7) ---

            // Crear un DTO de respuesta que contenga los detalles finales 
            var ofertaDetail = new OfertaDetailsDTO(
                nuevaOferta.Id,
                nuevaOferta.fechaInicio,
                nuevaOferta.fechaFinal,
                nuevaOferta.fechaOferta,
                nuevaOferta.MetodoPago,
                nuevaOferta.dirigidaA,
                nuevaOferta.ofertaItems.Select(oi => new OfertaItemDTO(
                    oi.porcentaje,
                    oi.precioFinal,
                    herramientas.First(h => h.Id == oi.herramientaId).Nombre,
                    herramientas.First(h => h.Id == oi.herramientaId).Material,
                    herramientas.First(h => h.Id == oi.herramientaId).fabricante,
                    herramientas.First(h => h.Id == oi.herramientaId).Precio
                )).ToList()
            );

            // Devuelve el código 201 Created y la ubicación del recurso recién creado
            return CreatedAtAction("GetOferta", new { id = nuevaOferta.Id }, ofertaDetail);
        }
    }
}
