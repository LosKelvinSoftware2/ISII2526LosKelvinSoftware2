using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.Comprar_Herramienta;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ComprasController_test
{
    public class PostCompra_test : AppForSEII25264SqliteUT
    {
        // Datos de prueba
        private const string _clienteNombre = "Maria";
        private const string _clienteApellido = "Lopez";
        private const double _clienteTelefono = 60033344;
        private const string _clienteCorreo = "marialopez@alu";
        private const string _direccionEnvio = "Avda. España s/n, Albacete 02071";

        private const string _herramienta1Nombre = "Taladro";
        private const string _herramienta1Material = "Metal";
        private const string _herramienta2Nombre = "Martillo";
        private const string _herramienta2Material = "Plástico";

        public PostCompra_test()
        {
            // --- CREAMOS LOS DATOS DE PRUEBA ---
            var fabricantes = new List<Fabricante>()
            {
                new Fabricante(){ Id=1, Nombre="Fabricante1"},
                new Fabricante(){ Id=2, Nombre="Fabricante2"},
            };

            var herramientas = new List<Herramienta>()
            {
                new Herramienta(){ Id=1, Nombre=_herramienta1Nombre, Material=_herramienta1Material, Precio=100.0f, TiempoReparacion=5, fabricante=fabricantes[0]},
                new Herramienta(){ Id=2, Nombre=_herramienta2Nombre, Material=_herramienta2Material, Precio=50.0f, TiempoReparacion=3, fabricante=fabricantes[1]},
            };

            ApplicationUser user = new ApplicationUser()
            {
                Id = "U001",
                Nombre = _clienteNombre,
                Apellido = _clienteApellido,
                PhoneNumber = _clienteTelefono.ToString(),
                correoelectronico = _clienteCorreo,
                UserName = _clienteCorreo,
                NormalizedUserName = _clienteCorreo.ToUpper(),
                NormalizedEmail = _clienteCorreo.ToUpper(),
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEHASHASHASHASHAS==",
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            // Creamos una compra ya existente en la DB para pruebas de Get
            var compra = new Compra()
            {
                Id = 1,
                fechaCompra = DateTime.UtcNow,
                PrecioTotal = 200.0f,
                MetodoPago = tiposMetodoPago.Efectivo,
                Cliente = user,
                direccionEnvio = _direccionEnvio,
                CompraItems = new List<CompraItem>()
                {
                    new CompraItem() { compraId = 1, herramientaId = 1, herramienta = herramientas[0], cantidad = 1, precio = 100.0f, descripcion = "Compra de Taladro" },
                    new CompraItem() { compraId = 1, herramientaId = 2, herramienta = herramientas[1], cantidad = 2, precio = 50.0f, descripcion = "Compra de Martillo" }
                }
            };

            _context.Fabricante.AddRange(fabricantes);
            _context.Herramienta.AddRange(herramientas);
            _context.ApplicationUser.Add(user);
            _context.Compra.Add(compra);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreateCompra()
        {
            var herramientasDTO = new List<Herramienta>()
            {
                new Herramienta(){ Id=1, Nombre="Taladro", Material="Metal", Precio=100.0f},
                new Herramienta(){ Id=2, Nombre="Martillo", Material="Plástico", Precio=50.0f}
            };

            // DTO sin herramientas
            var compraSinItems = new CompraDTO(_clienteNombre, _clienteApellido, _clienteTelefono, _clienteCorreo,
                _direccionEnvio, 0f, DateTime.Today.AddDays(1), new List<CompraItemDTO>(), tiposMetodoPago.Efectivo);

            // DTO con fecha anterior a hoy
            var compraFechaIncorrecta = new CompraDTO(_clienteNombre, _clienteApellido, _clienteTelefono, _clienteCorreo,
                _direccionEnvio, 100f, DateTime.Today.AddDays(-1),
                new List<CompraItemDTO> { new CompraItemDTO(herramientasDTO[0], 1, herramientasDTO[0].Precio) },
                tiposMetodoPago.TarjetaCredito);

            // DTO con cantidad inválida
            var compraCantidadIncorrecta = new CompraDTO(_clienteNombre, _clienteApellido, _clienteTelefono, _clienteCorreo,
                _direccionEnvio, 50f, DateTime.Today.AddDays(1),
                new List<CompraItemDTO> { new CompraItemDTO(herramientasDTO[1], 0, herramientasDTO[1].Precio) },
                tiposMetodoPago.PayPal);

            return new List<object[]>
            {
                new object[] { compraSinItems, "Debe haber al menos una herramienta para comprar" },
                new object[] { compraFechaIncorrecta, "La compra no puede realizarse en una fecha anterior a hoy" },
                new object[] { compraCantidadIncorrecta, "Debe especificarse una cantidad válida para cada herramienta" },
            };
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateCompra))]
        public async Task CreateCompra_Error_test(CompraDTO compraDTO, string errorExpected)
        {
            var mockLogger = new Mock<ILogger<CompraController>>();
            ILogger<CompraController> logger = mockLogger.Object;
            var controller = new CompraController(_context, logger);

            var result = await controller.CreateCompra(compraDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];
            Assert.StartsWith(errorExpected, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateCompra_Success_test()
        {
            var mockLogger = new Mock<ILogger<CompraController>>();
            ILogger<CompraController> logger = mockLogger.Object;
            var controller = new CompraController(_context, logger);

            // Tomamos herramientas existentes
            var herramientas = _context.Herramienta.Take(2).ToList();

            // Creamos CompraItemDTO válidos
            var compraItems = herramientas.Select((h, index) =>
                new CompraItemDTO(h, 1 + index, h.Precio) // cantidades > 0
            ).ToList();

            // DTO de compra con fecha futura
            var compraDTO = new CompraDTO(
                _clienteNombre,
                _clienteApellido,
                _clienteTelefono,
                _clienteCorreo,
                _direccionEnvio,
                0f, // precio total calculado en el controlador
                DateTime.Today.AddDays(1),
                compraItems,
                tiposMetodoPago.Efectivo
            );

            // Ejecutamos POST
            var result = await controller.CreateCompra(compraDTO);

            // Debe devolver CreatedAtActionResult con CompraDetailsDTO
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualCompraDetailDTO = Assert.IsType<CompraDetailsDTO>(createdResult.Value);

            // Opcional: validar acción y código de estado
            Assert.Equal(nameof(CompraController.GetCompraDetails), createdResult.ActionName);
            Assert.Equal((int)HttpStatusCode.Created, createdResult.StatusCode);

            // Validamos datos del cliente
            Assert.Equal(_clienteNombre, actualCompraDetailDTO.nombreCliente);
            Assert.Equal(_clienteApellido, actualCompraDetailDTO.apellidoCliente);
            Assert.Equal(_clienteCorreo, actualCompraDetailDTO.correoCliente);
            Assert.Equal(_clienteTelefono, actualCompraDetailDTO.telefonoCliente);
            Assert.Equal(_direccionEnvio, actualCompraDetailDTO.direccionEnvio);

            // Validamos items
            Assert.Equal(compraItems.Count, actualCompraDetailDTO.CompraItems.Count);
            for (int i = 0; i < compraItems.Count; i++)
            {
                Assert.Equal(compraItems[i].nombre, actualCompraDetailDTO.CompraItems[i].nombre);
                Assert.Equal(compraItems[i].material, actualCompraDetailDTO.CompraItems[i].material);
                Assert.Equal(compraItems[i].cantidad, actualCompraDetailDTO.CompraItems[i].cantidad);
            }
        }
    }
}
