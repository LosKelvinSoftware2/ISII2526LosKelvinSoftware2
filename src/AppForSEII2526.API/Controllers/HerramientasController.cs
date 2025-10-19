using AppForSEII2526.API.DTO;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HerramientasController : ControllerBase
    {
        //contorlador para la base datos
        private readonly ApplicationDbContext _context;
        // un log de problemas
        private readonly ILogger<HerramientasController> _logger;
        public HerramientasController(ApplicationDbContext context, ILogger<HerramientasController> logger)
        {
            _context = context;
            _logger = logger;

        }

        //Reparacion de herramientas Saelices
        [HttpGet]
        [Route("[action]")]
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

        // Crear Oferta, Telmo

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(OfertaDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]

        public async Task<ActionResult> GetOferta(int id)
        {
            if (_context.Oferta == null)
            {
                _logger.LogError("Error: Oferta table does not exist");
                return NotFound();
            }

            var oferta = await _context.Oferta
                .Where(o => o.Id == id)
                    .Include(o => o.ofertaItems) // join table OfertaItems
                        .ThenInclude(oi => oi.herramienta)
                .Select(o => new OfertaDTO(
                    o.Id,
                    o.fechaFinal,
                    o.fechaInicio,
                    o.fechaOferta,
                    o.ofertaItems
                        .Select(oi => new OfertaItemDTO(
                            oi.precioFinal,
                            oi.herramienta.Nombre,
                            oi.herramienta.Material,
                            oi.herramienta.fabricante.Nombre,
                            oi.herramienta.Precio
                            )).ToList,
                    o.metodoPago,
                    o.dirigidaA
                    ))
                .FirstOrDefaultAsync();

            if (oferta == null)
            {
                _logger.LogWarning($"No se encontró la oferta con id {id}");
                return NotFound();
            }

            return Ok(oferta);
        }
    }
}
