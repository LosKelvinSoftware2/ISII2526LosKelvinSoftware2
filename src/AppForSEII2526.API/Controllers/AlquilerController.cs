using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppForSEII2526.API.DTO;
using AppForSEII2526.API.DTO.Alquilar_Herramienta;
using AppForSEII2526.API.Models;
using System.Linq;


namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlquilerController : ControllerBase
    {
        //contorlador para la base datos
        private readonly ApplicationDbContext _context;
        // un log de problemas
        private readonly ILogger<HerramientasController> _logger;
        public AlquilerController(ApplicationDbContext context, ILogger<HerramientasController> logger)
        {
            _context = context;
            _logger = logger;

        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilerDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> GetAlquilerDetail(int id)
        {
            if (_context.Alquiler == null)
            {
                _logger.LogError("Error: Alquiler table does not exist");
                return NotFound();
            }
            var alquiler = await _context.Alquiler
                .Where(a => a.Id == id)
                .Select(a => new AlquilerDetailDTO(
                    a.Id,
                    a.Cliente.Nombre,
                    a.Cliente.Apellido,
                    a.direccionEnvio,
                    DateTime.Parse(a.fechaAlquiler),
                    DateTime.Parse(a.fechaFin),
                    a.precioTotal,
                    a.AlquilarItems.Select(ai => new AlquilarItemDTO(
                        ai.cantidad,
                        ai.precio,
                        ai.alquilerId,
                        ai.alquiler,
                        ai.herramientaId,
                        ai.herramienta
                    )).ToList()

                ))
                .FirstOrDefaultAsync();
            if (alquiler == null)
            {
                _logger.LogWarning($"Alquiler con id {id} no ha sido encontrado.");
                return NotFound();
            }
            return Ok(alquiler);
        }
    }
}
