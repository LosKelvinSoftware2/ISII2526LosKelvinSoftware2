using AppForSEII2526.API.DTO;
using AppForSEII2526.API.DTO.Alquilar_Herramienta;
using AppForSEII2526.API.DTO.Comprar_Herramienta;
using AppForSEII2526.API.DTO.OfertaDTOs;
using AppForSEII2526.API.DTO.RepararDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Necesario para EF
using System.Net; // Necesario para HttpStatusCode

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HerramientasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HerramientasController> _logger;
        public HerramientasController(ApplicationDbContext context, ILogger<HerramientasController> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogInformation("HerramientasController initialized");
        }

        //Comprar herramientas Javi (Mellado)   
        [HttpGet]
        [Route("DisponiblesCompra")]
        [ProducesResponseType(typeof(List<CompraHerramientasDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<CompraHerramientasDTO>>> GetHerramientasDisponibles(string? nombre, float? precio)
        {
            _logger.LogInformation($"Buscando herramientas para compra. Filtros -> Nombre: '{nombre ?? "Todos"}', Precio: {precio?.ToString() ?? "Cualquiera"}");

            var query = _context.Herramienta
                                .Include(h => h.fabricante)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(h => h.Nombre.Contains(nombre));

            if (precio.HasValue)
                query = query.Where(h => h.Precio == precio.Value);

            var herramientas = await query.ToListAsync();

            if (!herramientas.Any())
            {
                _logger.LogWarning("No se encontraron herramientas con los filtros proporcionados.");
                return NotFound(new { Mensaje = "No hay herramientas disponibles con los parámetros introducidos." });
            }

            _logger.LogInformation($"Se encontraron {herramientas.Count} herramientas.");
            
            var herramientasDTO = herramientas.Select(h => new CompraHerramientasDTO(
                h.Id,
                h.Nombre,
                h.Material,
                h.Precio,
                h.fabricante
            )).ToList();

            return Ok(herramientasDTO);
        }

        //Reparacion de herramientas Saelices PASO-2
        [HttpGet("DisponiblesReparacion")]
        [ProducesResponseType(typeof(List<HerramientaRepaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<HerramientaRepaDTO>>> GetHerramientasDisponiblesParaReparar(string? NombreHerramienta , int? DiaReparacion)
        {
            _logger.LogInformation($"Buscando herramientas reparables. Nombre: {NombreHerramienta ?? "*"}, Dias: {DiaReparacion?.ToString() ?? "*"}");

            if (_context.Herramienta == null)
            {
                _logger.LogError("CRITICAL: Herramienta table does not exist");
                return NotFound();
            }
            
            // Obtener las herramientas que NO están en reparaciones activas
            var herramientasEnReparacion = await _context.ReparacionItem
                .Select(ri => ri.herramientaId)
                .Distinct()
                .ToListAsync();

            var herramientas = await _context.Herramienta
                .Where(h => (NombreHerramienta == null || h.Nombre.Contains(NombreHerramienta)) 
                         && (DiaReparacion == null || h.TiempoReparacion == DiaReparacion)
                         && (herramientasEnReparacion.Contains(h.Id)) // NOTA: Tu lógica original dice "Contains", ¿no debería ser !Contains para "Disponibles"?
                                                                       // Mantengo tu lógica original, pero añado el log.
                )
                .Include(h => h.fabricante)
                .Select(h => new HerramientaRepaDTO(
                    h.Id,
                    h.Nombre,
                    h.Material,
                    h.fabricante.Nombre,
                    h.Precio,
                    h.TiempoReparacion
                ))
                .ToListAsync();

            if (!herramientas.Any())
            {
                _logger.LogWarning("No se encontraron herramientas según criterios de reparación.");
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
            _logger.LogInformation($"Buscando herramienta para oferta. Fab: {fabricante}, MaxPrecio: {precio}");

            if (precio < 0)
            {
                _logger.LogError($"Intento de búsqueda con precio negativo: {precio}");
                ModelState.AddModelError("precio", "El precio no puede ser negativo");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            if (_context.Herramienta == null)
            {
                _logger.LogError("Error: Herramienta table does not exist");
                return NotFound();
            }

            precio = !precio.HasValue ? 0 : precio;

            var herramienta = await _context.Herramienta
                .Where(h => ((fabricante == null) || (h.fabricante.Nombre == fabricante))
                    && ((precio == 0) || (h.Precio <= precio)))
                .Select(h => new HerramientaForOfertaDTO(h.Id, h.Nombre, h.Material, h.Precio, h.fabricante))
                .ToListAsync();

            if (herramienta == null || !herramienta.Any())
            {
                _logger.LogWarning($"No se encontró ninguna herramienta del fabricante {fabricante ?? "Cualquiera"}");
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
            var fechaInicio = DateTime.Today.AddDays(2);
            var fechaFin = DateTime.Today.AddDays(9);

            _logger.LogInformation($"Calculando disponibilidad alquileres entre {fechaInicio:d} y {fechaFin:d}. Filtros: {nombre}/{material}");

            if (_context.Alquiler == null)
            {
                _logger.LogError("Error: La tabla Alquiler no existe");
                return NotFound();
            }

            var herramientasOcupadas = await _context.Alquiler
               .Where(a => a.fechaAlquiler <= fechaFin && a.fechaFin >= fechaInicio)
               .SelectMany(a => a.AlquilarItems.Select(ai => ai.herramienta.Id))
               .Distinct()
               .ToListAsync();
            
            _logger.LogInformation($"Herramientas actualmente ocupadas en ese rango: {herramientasOcupadas.Count}");

            var herramientasDisponibles = await _context.Herramienta
                .Where(h => !herramientasOcupadas.Contains(h.Id) &&
                    (nombre == null || h.Nombre.Contains(nombre)) &&
                    (material == null || h.Material.Contains(material)))
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
                _logger.LogError("No hay herramientas disponibles con los parámetros introducidos.");
                ModelState.AddModelError("nombre&material", "No hay herramientas disponibles con los parámetros introducidos.");
                return NotFound(new ValidationProblemDetails(ModelState));
            }

            _logger.LogInformation($"Devolviendo {herramientasDisponibles.Count} herramientas disponibles.");
            return Ok(herramientasDisponibles);
        }
    }
}