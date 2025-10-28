using AppForSEII2526.API.DTO.Comprar_Herramienta;
using AppForSEII2526.API.DTO;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AppForSEII2526.API.DTO.Alquilar_Herramienta;

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



        //Comprar herramientas Javi (Mellado)   
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompraDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]

        public async Task<ActionResult> GetCompra(int id)
        {
            if (_context.Compra == null)
            {
                _logger.LogError("Error: Compras table does not exist");
                return NotFound();
            }
            var compra = await _context.Compra
                .Where(c => c.Id == id)                  // filtro por id
                .Include(c => c.CompraItems)             // join con CompraItem
                    .ThenInclude(ci => ci.herramienta)   // join con Herramienta
                .Select(c => new CompraDetailsDTO(
                    c.Id,
                    c.Cliente,                           // Nombre completo del cliente
                    c.direccionEnvio,
                    c.fechaCompra,
                    c.PrecioTotal,
                    c.CompraItems
                        .Select(ci => new CompraItemDTO( 
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
                _logger.LogWarning($"No se encontró la compra con id {id}");
                return NotFound();
            }
            return Ok(compra);
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

        //Alquilar herramienta, Juan Pe

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilerDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]

        public async Task<ActionResult> GetAlquiler(String nombre , String material)
        {
            if (_context.Alquiler == null)
            {
                _logger.LogError("Error: Alquiler table does not exist");
                return NotFound();
            }
            var alquiler = await _context.Alquiler
                .Include(a => a.AlquilarItems)
                    .ThenInclude(ai => ai.herramienta)
                .SelectMany(a => a.AlquilarItems)
                .Where(ai => ai.herramienta.Nombre == nombre && ai.herramienta.Material == material)
                .Select(ai => new AlquilerHerramientasDTO(
                    ai.alquiler.Id,
                    ai.herramienta.Nombre,
                    ai.herramienta.Material,
                    ai.precio,
                    ai.herramienta.fabricante))
                .ToListAsync();
            if (alquiler == null || !alquiler.Any())
            {
                return NotFound(new{Mensaje = "No se encontraron alquileres que coincidan con el nombre y material indicados."});
            }

            return Ok(alquiler);
        }


    }
}
