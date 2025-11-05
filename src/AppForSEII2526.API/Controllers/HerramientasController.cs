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
        [HttpGet]
        [Route("DisponiblesCompra")]
        [ProducesResponseType(typeof(List<CompraHerramientasDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<CompraHerramientasDTO>>> GetHerramientasDisponibles(string? material , float? precio)
        {
            if (_context.Herramienta == null)
            {
                _logger.LogError("Error: Herramienta table does not exist");
                return NotFound();
            }

            var herramientas = await _context.Herramienta

                .Where(h => (h.Material == material || material == null) && 
                            ((h.Precio <= precio) || precio == null) ) 
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

        //Reparacion de herramientas Saelices PASO-2
        [HttpGet("DisponiblesReparacion")]
        [ProducesResponseType(typeof(List<HerramientaRepaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<HerramientaRepaDTO>>> GetHerramientasDisponiblesParaReparar()
        {
            if (_context.Herramienta == null)
            {
                _logger.LogError("Error: Herramienta table does not exist");
                return NotFound();
            }

            var herramientas = await _context.Herramienta
                .Select(h => new HerramientaRepaDTO(
                    h.Id,
                    h.Nombre,
                    h.Material,
                    h.Precio,
                    h.TiempoReparacion,
                    h.fabricante
                ))
                .ToListAsync();

            if (!herramientas.Any())
            {
                return NotFound(new { Mensaje = "No hay herramientas disponibles para reparación." });
            }

            return Ok(herramientas);
        }

        // Crear Oferta, Telmo

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(OfertaDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]

        public async Task<ActionResult> GetHerramientaForOferta(string? fabricante, float? precio)
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
        [ProducesResponseType(typeof(List<AlquilerHerramientasDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]

        public async Task<ActionResult> GetAlquileresDisponibles(string? nombre, string? material)
        {
            if (_context.Alquiler == null)
            {
                _logger.LogError("Error: Alquiler table does not exist");
                return NotFound();
            }

            var fechaInicio = DateTime.Today.AddDays(2); //Alquileres a partir de pasado mañana
            var fechaFin = DateTime.Today.AddDays(9); //Alquileres hasta una semana después del inicio

            var herramientasOcupadas = await _context.Alquiler
           .Where(a => a.fechaAlquiler <= fechaFin && a.fechaFin >= fechaInicio)    // Alquileres que se solapan con el periodo dado
           .SelectMany(a => a.AlquilarItems.Select(ai => ai.herramienta.Id))        //Guardamos los Id de las herramientas ya alquiladas en dicho periodo
           .Distinct()  //Eliminamos duplicados
           .ToListAsync();

            // Ahora obtenemos las herramientas que no están en la lista de ocupadas
            var herramientasDisponibles = await _context.Herramienta
                .Where(h => !herramientasOcupadas.Contains(h.Id) &&
                    (nombre == null || h.Nombre.Contains(nombre)) &&
                    (material == null || h.Material == material))
                .Select(h => new AlquilerHerramientasDTO
                (
                    h.Id,
                    h.Nombre,
                    h.Material,
                    h.Precio,
                    h.fabricante
                ))
                .ToListAsync();

            if (!herramientasDisponibles.Any())
            {
                return NotFound(new { Mensaje = "No hay herramientas disponibles para alquilar en las fechas indicadas." });
            }

            return Ok(herramientasDisponibles);
        }


    }
}