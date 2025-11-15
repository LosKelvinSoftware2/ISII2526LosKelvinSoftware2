using AppForSEII2526.API.DTO.OfertaDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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

            _logger.LogInformation("OfertasController initialized");
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
                    o.porcentaje,
                    o.fechaInicio,
                    o.fechaFinal,
                    o.fechaOferta,
                    o.metodoPago,
                    o.dirigidaA,
                    o.ofertaItems.Select(oi => new OfertaItemDTO(
                        oi.herramientaId,
                        oi.precioFinal,
                        oi.herramienta.Nombre,
                        oi.herramienta.Material,
                        oi.herramienta.fabricante.Nombre,
                        oi.herramienta.Precio
                    )).ToList<OfertaItemDTO>()
                ))
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
            if (ofertaForCreate.fechaInicio <= DateTime.Today)
                ModelState.AddModelError("RentalDateFrom", "Error! La fecha de inicio de oferta debe ser al menos mañana");
            if (ofertaForCreate.fechaInicio >= ofertaForCreate.fechaFinal)
                ModelState.AddModelError("RentalDateTo", "Error! La fecha final de oferta debe ser después de la fecha de inicio");
            if (ofertaForCreate.porcentaje <= 0 || ofertaForCreate.porcentaje > 100)
                ModelState.AddModelError("porcentaje", "Error! El porcentaje de descuento debe estar entre 1 y 100");
            if (ofertaForCreate.ofertaItems.Count == 0)
                ModelState.AddModelError("RentalItems", "Error! Hay que incluir al menos una herramienta");


            // Retorno si hay errores iniciales
            if (!ModelState.IsValid)
            {
                // Flujo Alternativo 5: Datos obligatorios no rellenados (o rango incorrecto)
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // --- 2. Validación de Existencia de Herramientas ---
            var herramientaIds = ofertaForCreate.ofertaItems.Select(oi => oi.herramientaId).Distinct().ToList();

            // Obtener los datos de las herramientas necesarias (Nombre, Material, Fabricante, Precio Original)
            var herramientas = await _context.Herramienta
                .Include(h => h.fabricante) 
                .Where(h => herramientaIds.Contains(h.Id))
                .ToDictionaryAsync(h => h.Id);


            // --- 3. Construcción de Entidades (Modelos de DB) ---

            // Crear la entidad Oferta principal
            var nuevaOferta = new Oferta
            {
                porcentaje = ofertaForCreate.porcentaje,
                fechaInicio = ofertaForCreate.fechaInicio,
                fechaFinal = ofertaForCreate.fechaFinal,
                fechaOferta = DateTime.Today, // La fecha de creación es ahora
                metodoPago = ofertaForCreate.metodoPago,
                dirigidaA = ofertaForCreate.dirigidaA,
                // Las propiedades de precio/porcentaje pueden variar.
            };
            // Crear las entidades OfertaItem y adjuntarlas a la Oferta
            foreach (var item in ofertaForCreate.ofertaItems)
            {
                var herramienta = herramientas[item.herramientaId];

                // Cálculo del precio con la oferta (asumiendo que Precio en Herramienta es el original)
                float precioOriginal = herramienta.Precio;
                float porcentaje = ofertaForCreate.porcentaje / 100.0f; // Convertir a decimal
                float precioFinal = precioOriginal - (precioOriginal * porcentaje);

                nuevaOferta.ofertaItems.Add(new OfertaItem
                {
                    herramientaId = item.herramientaId,
                    porcentaje = ofertaForCreate.porcentaje, // Guardamos el porcentaje en DB si es necesario
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
                nuevaOferta.porcentaje,
                nuevaOferta.fechaInicio,
                nuevaOferta.fechaFinal,
                nuevaOferta.fechaOferta,
                nuevaOferta.metodoPago,
                nuevaOferta.dirigidaA,
                nuevaOferta.ofertaItems.Select(oi => new OfertaItemDTO(
                    oi.herramientaId,
                    oi.precioFinal,
                    herramientas[oi.herramientaId].Nombre,
                    herramientas[oi.herramientaId].Material,
                    herramientas[oi.herramientaId].fabricante.Nombre,
                    herramientas[oi.herramientaId].Precio
            )).ToList());

            // Devuelve el código 201 Created y la ubicación del recurso recién creado
            return CreatedAtAction("GetOferta", new { id = nuevaOferta.Id }, ofertaDetail);
        }
    }
}
