using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.Comprar_Herramienta;
using AppForSEII2526.API.DTO.OfertaDTOs;
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
   /* public class PostCompra_test : AppForSEII25264SqliteUT
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

            // Compra de ejemplo ya existente
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

        // --- CASOS DE ERROR ---
        public static IEnumerable<object[]> TestCasesFor_CreateCompra()
        {
            // CompraItems válidos que existen en la DB
            var compraItemsValid = new List<CompraItemDTO>()
            {
                new CompraItemDTO(1, "Taladro", "Metal", 1, 100),
                new CompraItemDTO(2, "Martillo", "Plástico", 1, 50)
            };

            // 1️ Sin items
            var compraSinItems = new CompraDTO(
                _clienteNombre, _clienteApellido, _clienteTelefono, _clienteCorreo,
                _direccionEnvio, 0f, "a",  DateTime.Today.AddDays(1),
                new List<CompraItemDTO>(), tiposMetodoPago.Efectivo
            );

            // 2️ Fecha anterior a hoy
            var compraFechaIncorrecta = new CompraDTO(
                _clienteNombre, _clienteApellido, _clienteTelefono, _clienteCorreo,
                _direccionEnvio, 150f, "a",  DateTime.Today.AddDays(-1),
                new List<CompraItemDTO> { compraItemsValid[0] },
                tiposMetodoPago.TarjetaCredito
            );

            // 3️ Cantidad inválida
            var compraCantidadIncorrecta = new CompraDTO(
                _clienteNombre, _clienteApellido, _clienteTelefono, _clienteCorreo,
                _direccionEnvio, 50f,"a", DateTime.Today.AddDays(1),
                new List<CompraItemDTO> { new CompraItemDTO(2, "Martillo", "Plástico", 0, 50) },
                tiposMetodoPago.PayPal
            );

            // Examen Sin descripcion
            var compraSinDescripcion = new CompraDTO(
                _clienteNombre, _clienteApellido, _clienteTelefono, _clienteCorreo,
                _direccionEnvio, 50f, "", DateTime.Today.AddDays(1),
                new List<CompraItemDTO> { new CompraItemDTO(2, "Martillo", "Plástico", 0, 50) },
                tiposMetodoPago.PayPal
            );

            return new List<object[]>
            {
                new object[] { compraSinItems, "Debe haber al menos una herramienta para comprar" },
                new object[] { compraFechaIncorrecta, "La compra no puede realizarse en una fecha anterior a hoy" },
                new object[] { compraCantidadIncorrecta, "Debe especificarse una cantidad válida para cada herramienta" },
                new object[] { compraSinDescripcion, "¡Error! Estas comprando demasiadas herramientas sin descripcion." }
            };
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateCompra))]
        public async Task CreateCompra_Error_test(CompraDTO compraDTO, string errorExpected)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CompraController>>();
            ILogger<CompraController> logger = mockLogger.Object;
            var controller = new CompraController(_context, logger);

            // Act
            var result = await controller.CreateCompra(compraDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(errorExpected, errorActual);
        }

        // --- SUCCESS TEST SE MANTIENE IGUAL ---
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateCompra_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CompraController>>();
            ILogger<CompraController> logger = mock.Object;
            var controller = new CompraController(_context, logger);

            var compraItems = new List<CompraItemDTO>()
            {
            new CompraItemDTO(1, "Taladro", "Metal", 1, 100.0f),
            new CompraItemDTO(2, "Martillo", "Plástico", 1, 50.0f)
            };

            var compraForCreate = new CompraDTO
            (
                nombreCliente: _clienteNombre,
                apellidoCliente: _clienteApellido,
                telefonoCliente: _clienteTelefono,
                correoCliente: _clienteCorreo,
                direccionEnvio: _direccionEnvio,
                PrecioTotal: 150.0f,
                descripcion: "a",
                fechaCompra: DateTime.Today,
                CompraItemDTO: compraItems,
                MetodoPago: tiposMetodoPago.TarjetaCredito
            );

            // Act
            var result = await controller.CreateCompra(compraForCreate);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdCompra = Assert.IsType<CompraDetailsDTO>(createdAtActionResult.Value);

            // Comparamos propiedades individuales
            Assert.Equal(_clienteNombre, createdCompra.nombreCliente);
            Assert.Equal(_clienteApellido, createdCompra.apellidoCliente);
            Assert.Equal(_clienteCorreo, createdCompra.correoCliente);
            Assert.Equal(_direccionEnvio, createdCompra.direccionEnvio);
            Assert.Equal(compraForCreate.PrecioTotal, createdCompra.PrecioTotal);
            Assert.Equal(compraForCreate.MetodoPago, createdCompra.MetodoPago);

            // Comparamos fecha solo por día
            Assert.Equal(DateTime.UtcNow.Date, createdCompra.fechaCompra.Date);

            // Comparamos los items
            Assert.Equal(compraItems.Count, createdCompra.CompraItems.Count);
            for (int i = 0; i < compraItems.Count; i++)
            {
                Assert.Equal(compraItems[i].herramientaId, createdCompra.CompraItems[i].herramientaId);
                Assert.Equal(compraItems[i].nombre, createdCompra.CompraItems[i].nombre);
                Assert.Equal(compraItems[i].material, createdCompra.CompraItems[i].material);
                Assert.Equal(compraItems[i].cantidad, createdCompra.CompraItems[i].cantidad);
                Assert.Equal(compraItems[i].precio, createdCompra.CompraItems[i].precio);
            }
        }


    }*/
}
