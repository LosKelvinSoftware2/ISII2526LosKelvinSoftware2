using AppForSEII2526.API.DTO;
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





    }
}
