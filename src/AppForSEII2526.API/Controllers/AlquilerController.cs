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
                    a.fechaAlquiler,
                    a.fechaFin,
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

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilerDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateAlquiler(AlquilerHerramientasDTO alquilerForCreate)
        {
            foreach (var item in alquilerForCreate.AlquilerItems)
            {
                //Comprobamos que el alquiler se está realizando en una fecha correcta
                if (item.alquiler.fechaAlquiler <= DateTime.Today)
                    ModelState.AddModelError("AlquilerItems.FechaInicio", "Error! La fecha de inicio debe ser posterior a hoy.");

                if (item.alquiler.fechaAlquiler >= item.alquiler.fechaFin)
                    ModelState.AddModelError("AlquilerItems.FechaInicio/FechaFin", "La fecha de fin debe ser posterior a la de inicio.");
            }
                //Comprobamos que hay al menos una herramienta
            if (alquilerForCreate.AlquilerItems.Count == 0)
                ModelState.AddModelError("AlquilerItems", "No hay ningún artículo para crear el alquiler");


            var herramientasNombre = alquilerForCreate.AlquilerItems.Select(ri => ri.herramienta.Nombre).ToList<string>();

            var herramientas = _context.Herramienta.Include(h => h.AlquilarItems)
                .ThenInclude(ri => ri.alquiler)
                .Where(h => h.AlquilarItems.Any(ri => herramientasNombre.Contains(ri.herramienta.Nombre)))
                .Select(h => new {
                    h.Id,
                    h.Nombre,
                    h.Material,
                    h.Precio,

                    herramientasAlquiladas = h.AlquilarItems.Count(ri => ri.alquiler.fechaAlquiler <= alquilerForCreate.fechaFin
                    && ri.alquiler.fechaFin >= alquilerForCreate.fechaInicio)
                })
                .ToList();

            Alquiler alquiler = new Alquiler(alquilerForCreate.direccionEnvio , alquilerForCreate.fechaInicio, alquilerForCreate.fechaFin,
                alquilerForCreate.Id , alquilerForCreate.Precio, new List<AlquilarItem> (),alquilerForCreate.metodoPago ,
                alquilerForCreate.nombreCliente , alquilerForCreate.apellidoCliente );

            alquiler.precioTotal = 0;
            var numDays = (alquiler.fechaFin - alquiler.fechaAlquiler).TotalDays;


            foreach (var item in alquilerForCreate.AlquilerItems)
            {
                var herramienta = herramientas.FirstOrDefault(h => h.Nombre == item.herramienta.Nombre);
                alquiler.AlquilarItems.Add(new AlquilarItem(item.precio, alquiler.Id));
                item.precio = herramienta.Precio;
                
            }
            alquiler.precioTotal = (float)alquiler.AlquilarItems.Sum(ri => ri.precio * numDays);

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(alquiler);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Rental", $"Error! There was an error while saving your rental, plese, try again later");
                return Conflict("Error" + ex.Message);

            }
 

        }
    }

}
   
      
