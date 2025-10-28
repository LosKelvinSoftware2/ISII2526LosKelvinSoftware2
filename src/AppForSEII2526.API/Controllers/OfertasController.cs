using AppForSEII2526.API.DTO.OfertaDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                    o.fechaInicio,
                    o.fechaFinal,
                    o.fechaOferta,
                    o.metodoPago,
                    o.dirigidaA,
                    o.ofertaItems.Select(oi => new OfertaItemDTO(
                        oi.ofertaId,
                        oi.porcentaje,
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

    }
}
