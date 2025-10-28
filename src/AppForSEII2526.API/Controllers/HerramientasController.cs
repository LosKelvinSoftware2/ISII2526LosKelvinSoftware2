using AppForSEII2526.API.DTO;
using AppForSEII2526.API.DTO.OfertaDTOs;
using AppForSEII2526.API.DTO.Comprar_Herramienta;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AppForSEII2526.API.DTO.Alquilar_Herramienta;
using AppForSEII2526.API.DTO.RepararDTOs;

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
        [HttpGet("Disponibles")]
        [ProducesResponseType(typeof(List<CompraHerramientasDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CompraHerramientasDTO>>> GetHerramientasDisponibles()
        {
            if (_context.Herramienta == null)
            {
                _logger.LogError("Error: Herramienta table does not exist");
                return NotFound();
            }

            var herramientas = await _context.Herramienta
                .Select(h => new CompraHerramientasDTO(
                    h.Id,
                    h.Nombre,
                    h.Material,
                    h.Precio,
                    h.fabricante
                ))
                .ToListAsync();

            return Ok(herramientas);
        }



        //Reparacion de herramientas Saelices
        

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
                    ai.herramienta.fabricante,
                    ai.alquiler.fechaAlquiler,
                    ai.alquiler.fechaFin,
                    ai.alquiler.direccionEnvio,
                    ai.alquiler.MetodoPago,
                    ai.alquiler.nombreCliente,
                    ai.alquiler.apellidoCliente))
                .ToListAsync();
            if (alquiler == null || !alquiler.Any())
            {
                return NotFound(new{Mensaje = "No se encontraron alquileres que coincidan con el nombre y material indicados."});
            }

            return Ok(alquiler);
        }


    }
}
