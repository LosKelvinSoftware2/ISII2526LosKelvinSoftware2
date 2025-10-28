
using AppForSEII2526.API.DTO.Comprar_Herramienta;
using AppForSEII2526.API.DTO;
using AppForSEII2526.API.DTO.OfertaDTOs;
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

        public async Task<ActionResult> GetHerramientaForOferta (string? fabricante, float? precio)
        {
            if (_context.Herramienta == null)
            {
                _logger.LogError("Error: Herramienta table does not exist");
                return NotFound();
            }

            // Dar valor por defecto al precio si no se ha pasado por parámetro
            precio = !precio.HasValue ? 0 : precio;

            var herramienta = await _context.Herramienta
                .Where(h => // Filtrar por id, fabricante y precio
                    // Mostrar todas las herramientas si no se ha pasado el fabricante por parámetro
                    ((fabricante == null) || (h.fabricante.Nombre == fabricante))
                    && ((precio == 0) || (h.Precio <= precio))) // igual para precio si es 0
                .Select(h => new HerramientaForOfertaDTO(h.Id, h.Nombre, h.Material, h.Precio, h.fabricante))
                .ToListAsync();


            if (herramienta == null)
            {
                _logger.LogWarning($"No se encontró la herramienta con fabricante {fabricante}");
                return NotFound();
            }

            return Ok(herramienta);
        }

        //Alquilar herramienta, Juan Pe

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilerDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]

        public async Task<ActionResult> GetAlquiler(int id)
        {
            if (_context.Alquiler == null)
            {
                _logger.LogError("Error: Alquiler table does not exist");
                return NotFound();
            }
            var alquiler = await _context.Alquiler
                .Where(a => a.Id == id)
                .Include(a => a.AlquilarItems)         // join con AlquilerItem
                    .ThenInclude(ai => ai.herramienta)   // join con Herramienta
                .Select(a => new AlquilerDTO(
                    a.direccionEnvio,
                    a.fechaAlquiler,
                    a.fechaFin,
                    a.Id,
                    a.periodo,
                    a.precioTotal,
                    a.Cliente,                           // Nombre completo del cliente
                    a.AlquilarItems
                        .Select(ai => new AlquilarItemDTO(
                            ai.cantidad,
                            ai.herramienta.Precio,
                            ai.alquilerId,
                            ai.alquiler,
                            ai.herramientaId,
                            ai.herramienta
                        )).ToList()
                    , a.MetodoPago
                ))
                .FirstOrDefaultAsync();
            if (alquiler == null)
            {
                _logger.LogWarning($"No se encontró el alquiler con id {id}");
                return NotFound();
            }
            return Ok(alquiler);
        }

    }
}
