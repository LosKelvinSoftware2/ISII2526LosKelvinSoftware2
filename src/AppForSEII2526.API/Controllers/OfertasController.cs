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
                .Where (o => o.Id == id)
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
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
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

            var herramientasIds = ofertaForCreate.ofertaItems.Select(oi => oi.herramientaId).ToList();

            var herramienta = _context.Herramienta.Include(h => h.Ofertaitems)
                .ThenInclude(ri => ri.oferta)
                .Where(h => herramientasIds.Contains(h.Id))

                //we use an anonymous type https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/anonymous-types
                .Select(h => new {
                    h.Id,
                    h.Nombre,
                    h.Material,
                    h.Precio,
                    //we count the number of rentalItems that are within the rental period
                    NumHerramientasOfertadas = h.Ofertaitems.Count(oi => oi.oferta.fechaInicio <= ofertaForCreate.fechaFinal
                            && ofertaForCreate.fechaFinal >= oi.oferta.fechaInicio)
                })
                .ToList();

            Oferta oferta = new Oferta
            {
                porcentaje = ofertaForCreate.porcentaje,
                fechaFinal = ofertaForCreate.fechaFinal,
                fechaInicio = ofertaForCreate.fechaInicio,
                fechaOferta = DateTime.Today,
                metodoPago = ofertaForCreate.metodoPago,
                dirigidaA = ofertaForCreate.dirigidaA
            };

            _context.Oferta.Add(oferta);

            try {
                //we store in the database both rental and its rentalitems
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Oferta", $"Error! There was an error while saving your oferta, plese, try again later");
                return Conflict("Error" + ex.Message);

            }

            //it returns rentalDetail
            OfertaDetailsDTO OfertaDetail = new OfertaDetailsDTO
                (oferta.Id,
                oferta.porcentaje,
                oferta.fechaInicio,
                oferta.fechaFinal,
                oferta.fechaOferta,
                oferta.metodoPago,
                oferta.dirigidaA,
                oferta.ofertaItems.Select(oi => new OfertaItemDTO(
                    oi.precioFinal,
                    oi.herramienta.Nombre,
                    oi.herramienta.Material,
                    oi.herramienta.fabricante.Nombre,
                    oi.herramienta.Precio
                )).ToList<OfertaItemDTO>()

                );

            return CreatedAtAction("GetOferta", new { id = oferta.Id }, OfertaDetail);
        }
    }
}
