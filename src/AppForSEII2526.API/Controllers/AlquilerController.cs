using AppForSEII2526.API.DTO;
using AppForSEII2526.API.DTO.Alquilar_Herramienta;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net; // Añadido

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlquilerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AlquilerController> _logger;

        public AlquilerController(ApplicationDbContext context, ILogger<AlquilerController> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogInformation("AlquilerController initialized");
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilerDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAlquilerDetail(int id)
        {
            _logger.LogInformation($"Consultando detalle de Alquiler ID: {id}");

            if (_context.Alquiler == null)
            {
                _logger.LogCritical("Error CRITICO: Tabla Alquiler no existe.");
                return NotFound();
            }

            var alquiler = await _context.Alquiler
                .Where(a => a.Id == id)
                .Include(a => a.AlquilarItems)
                    .ThenInclude(ai => ai.herramienta)
                .Select(a => new AlquilerDetailDTO(
                    a.Id,
                    a.fechaAlquiler,
                    a.Cliente,
                    a.direccionEnvio,
                    a.precioTotal,
                    a.fechaFin,
                    a.fechaInicio,
                    a.AlquilarItems.Select(ai => new AlquilarItemDTO(
                        ai.herramienta,
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
            _logger.LogInformation($"Recibida solicitud de Alquiler. Inicio: {alquilerForCreate.fechaInicio:d} - Fin: {alquilerForCreate.fechaFin:d}");

            // Comprobamos que el alquiler se está realizando en una fecha correcta
            if (alquilerForCreate.fechaInicio <= DateTime.Today)
            {
                _logger.LogWarning($"Fecha Inicio inválida ({alquilerForCreate.fechaInicio}). Debe ser futura.");
                ModelState.AddModelError("fechaInicio", "El alquiler debe empezar después de hoy");
            }
            if (alquilerForCreate.fechaFin <= alquilerForCreate.fechaInicio)
            {
                _logger.LogWarning("Fecha Fin es anterior o igual a Fecha Inicio.");
                ModelState.AddModelError("fechaFin y fechaInicio", "El alquiler debe terminar después de iniciar");
            }

            // Comprobamos que hay al menos una herramienta
            if (alquilerForCreate.AlquilarItems.Count == 0)
                ModelState.AddModelError("AlquilarItems", "Debe haber al menos una herramienta a alquilar");

            // Validar datos del cliente
            if (string.IsNullOrEmpty(alquilerForCreate.Cliente.Nombre)) ModelState.AddModelError("Cliente.Nombre", "El nombre es obligatorio");
            if (string.IsNullOrEmpty(alquilerForCreate.Cliente.Apellido)) ModelState.AddModelError("Cliente.Apellido", "El apellido es obligatorio");
            if (string.IsNullOrEmpty(alquilerForCreate.direccionEnvio)) ModelState.AddModelError("direccionEnvio", "La dirección de envío es obligatoria");
            if (!Enum.IsDefined(typeof(tiposMetodoPago), alquilerForCreate.MetodoPago)) ModelState.AddModelError("metodoPago", "El método de pago es obligatorio");

            foreach (var item in alquilerForCreate.AlquilarItems)
            {
                if (item.cantidad <= 0)
                    ModelState.AddModelError("cantidad", "Debe especificarse una cantidad válida para cada herramienta");
            }

            if (ModelState.ErrorCount > 0)
            {
                _logger.LogWarning($"Validación fallida con {ModelState.ErrorCount} errores.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var herramientasNombre = alquilerForCreate.AlquilarItems.Select(ri => ri.nombreHerramienta).ToList<string>();
            var herramientasLista = await _context.Herramienta
                .Where(h => herramientasNombre.Contains(h.Nombre))
                .ToListAsync();

            Alquiler alquiler = new Alquiler
            {
                Cliente = alquilerForCreate.Cliente,
                direccionEnvio = alquilerForCreate.direccionEnvio,
                fechaAlquiler = DateTime.Today,
                fechaInicio = alquilerForCreate.fechaInicio,
                fechaFin = alquilerForCreate.fechaFin,
                MetodoPago = alquilerForCreate.MetodoPago,
                AlquilarItems = new List<AlquilarItem>()
            };

            alquiler.precioTotal = 0;
            
            // Nota: numDays se calculaba pero no se usaba en el precio en el código original, 
            // aunque sería lógico usarlo. Lo dejo igual pero añado Log.
            double numDays = (alquiler.fechaFin - alquiler.fechaInicio).TotalDays;
            _logger.LogInformation($"Duración del alquiler: {numDays} días.");

            foreach (var item in alquilerForCreate.AlquilarItems)
            {
                var herramienta = herramientasLista.FirstOrDefault(h => h.Nombre == item.nombreHerramienta);
                if (herramienta == null)
                {
                    _logger.LogWarning($"Herramienta '{item.nombreHerramienta}' no encontrada para alquiler.");
                    continue;
                }

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
                _logger.LogInformation($"Alquiler registrado exitosamente. ID: {alquiler.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando Alquiler en BD.");
                ModelState.AddModelError("Alquiler", $"Error! Ha ocurrido un error");
                return Conflict("Error" + ex.Message);
            }

            var alquilerDTO = new AlquilerDetailDTO(
                alquiler.Id,
                alquiler.fechaAlquiler,
                alquiler.Cliente,
                alquiler.direccionEnvio,
                alquiler.precioTotal,
                alquiler.fechaFin,
                alquiler.fechaInicio,
                alquiler.AlquilarItems.Select(ai => new AlquilarItemDTO(
                    ai.herramienta,
                    ai.cantidad,
                    ai.precio
                )).ToList(),
                alquiler.MetodoPago
            );

            return CreatedAtAction(nameof(GetAlquilerDetail), new { id = alquiler.Id }, alquilerDTO);
        }
    }
}