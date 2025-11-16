using AppForSEII2526.API.DTO;
using AppForSEII2526.API.DTO.Alquilar_Herramienta;
using AppForSEII2526.API.DTO.Comprar_Herramienta;
using AppForSEII2526.API.DTO.RepararDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ILogger<AlquilerController> _logger;
        public AlquilerController(ApplicationDbContext context, ILogger<AlquilerController> logger)
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
                .Include(a => a.AlquilarItems)
                    .ThenInclude(ai => ai.herramienta)
                .Select(a => new AlquilerDetailDTO(
                    a.Id,
                    a.fechaAlquiler,
                    a.Cliente.Nombre,
                    a.Cliente.Apellido,
                    a.direccionEnvio,
                    a.precioTotal,
                    a.fechaFin,
                    a.fechaInicio,
                    a.AlquilarItems.Select(ai => new AlquilarItemDTO(
                        ai.herramienta.Nombre,
                        ai.herramienta.Material,
                        ai.cantidad,
                        ai.precio
                    )).ToList(),
                    a.MetodoPago
                ))
                .FirstOrDefaultAsync();

            if (alquiler == null)
            {
                _logger.LogWarning($"Alquiler con id {id} no encontrada.");
                return NotFound();
            }

            return Ok(alquiler);



        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilerDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateAlquiler(AlquilerDTO alquilerForCreate)
        {
            //Comprobamos que el alquiler se está realizando en una fecha correcta
            if (alquilerForCreate.fechaInicio <= DateTime.Today)
                ModelState.AddModelError("fechaInicio", "El alquiler debe empezar después de hoy");
            if (alquilerForCreate.fechaFin <= alquilerForCreate.fechaInicio)
                ModelState.AddModelError("fechaFin y fechaInicio", "El alquiler debe terminar después de iniciar");
            //Comprobamos que hay al menos una herramienta
            if (alquilerForCreate.AlquilarItems.Count == 0)
                ModelState.AddModelError("AlquilarItems", "Debe haber al menos una herramienta a alquilar");
            // Validar datos del cliente
            if (string.IsNullOrEmpty(alquilerForCreate.nombreCliente))
                ModelState.AddModelError("Cliente.Nombre", "El nombre es obligatorio");
            if (string.IsNullOrEmpty(alquilerForCreate.apellidoCliente))
                ModelState.AddModelError("Cliente.Apellido", "El apellido es obligatorio");
            if (string.IsNullOrEmpty(alquilerForCreate.direccionEnvio))
                ModelState.AddModelError("direccionEnvio", "La dirección de envío es obligatoria");
            if (!Enum.IsDefined(typeof(tiposMetodoPago), alquilerForCreate.MetodoPago))
                ModelState.AddModelError("metodoPago", "El método de pago es obligatorio");

            var cliente = _context.ApplicationUser.FirstOrDefault(u => u.Nombre == alquilerForCreate.nombreCliente && u.Apellido == alquilerForCreate.apellidoCliente);
            // Validar cantidad de cada herramienta
            foreach (var item in alquilerForCreate.AlquilarItems)
            {
                if (item.cantidad <= 0) // Asegurarse de que la cantidad es positiva y que ha sido seleccionada la cantidad
                    ModelState.AddModelError("cantidad", "Debe especificarse una cantidad válida para cada herramienta");
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var herramientasNombre = alquilerForCreate.AlquilarItems.Select(ri => ri.nombreHerramienta).ToList<string>();
            var herramientasLista = await _context.Herramienta
                .Where(h => herramientasNombre.Contains(h.Nombre))
                .ToListAsync();

            Alquiler alquiler = new Alquiler
            {
                Cliente = cliente,
                direccionEnvio = alquilerForCreate.direccionEnvio,
                fechaAlquiler = DateTime.Today,
                fechaInicio = alquilerForCreate.fechaInicio,
                fechaFin = alquilerForCreate.fechaFin,
                MetodoPago = alquilerForCreate.MetodoPago,
                AlquilarItems = new List<AlquilarItem>()
            };

            alquiler.precioTotal = 0;
            double numDays = (alquiler.fechaFin - alquiler.fechaInicio).TotalDays;


            foreach (var item in alquilerForCreate.AlquilarItems)
            {
                var herramienta = herramientasLista.FirstOrDefault(h => h.Nombre == item.nombreHerramienta);
                if (herramienta == null) continue;

                var precioUnitario = herramienta.Precio;
                alquiler.AlquilarItems.Add(new AlquilarItem
                {
                    cantidad = item.cantidad,
                    precio = precioUnitario,
                    herramientaId = herramienta.Id,
                    herramienta = herramienta
                });
                alquiler.precioTotal += (float)(precioUnitario * item.cantidad);

            }

            _context.Alquiler.Add(alquiler);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Alquiler", $"Error! Ha ocurrido un error");
                return Conflict("Error" + ex.Message);

            }

            var alquilerDTO = new AlquilerDetailDTO(
                alquiler.Id,
                alquiler.fechaAlquiler,
                alquilerForCreate.nombreCliente,
                alquilerForCreate.apellidoCliente,
                alquiler.direccionEnvio,
                alquiler.precioTotal,
                alquiler.fechaFin,
                alquiler.fechaInicio,
                alquiler.AlquilarItems.Select(ai => new AlquilarItemDTO(
                    ai.herramienta.Nombre,
                    ai.herramienta.Material,
                    ai.cantidad,
                    ai.precio
                )).ToList(),
                alquiler.MetodoPago
            );

            return CreatedAtAction(nameof(GetAlquilerDetail), new { id = alquiler.Id }, alquilerDTO);
        }
    }

}
