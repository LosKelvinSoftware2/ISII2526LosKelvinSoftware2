using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.RepararDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ReparacionController_test
{
    public class PostReparacion_test : AppForSEII25264SqliteUT
    {
        // Datos de prueba constantes
        private const string _userName = "marialopez@alu";
        private const string _customerName = "Maria";
        private const string _customerSurname = "Lopez";
        private const string _phoneNumber = "+3460033344";

        public PostReparacion_test()
        {
            // Limpiar la base de datos primero
            _context.ReparacionItem.RemoveRange(_context.ReparacionItem);
            _context.Reparacion.RemoveRange(_context.Reparacion);
            _context.Herramienta.RemoveRange(_context.Herramienta);
            _context.Fabricante.RemoveRange(_context.Fabricante);
            _context.ApplicationUser.RemoveRange(_context.ApplicationUser);
            _context.SaveChanges();

            // Crear fabricantes
            var fabricantes = new List<Fabricante>()
    {
        new Fabricante { Id = 1, Nombre = "Fabricante1" },
        new Fabricante { Id = 2, Nombre = "Fabricante2" },
        new Fabricante { Id = 3, Nombre = "Fabricante3" }
    };

            // Crear herramientas
            var herramientas = new List<Herramienta>()
    {
        new Herramienta
        {
            Id = 1,
            Nombre = "Taladro",
            Material = "Metal",
            Precio = 100.0f,
            TiempoReparacion = 5,
            fabricante = fabricantes[0]
        },
        new Herramienta
        {
            Id = 2,
            Nombre = "Martillo",
            Material = "Plástico",
            Precio = 50.0f,
            TiempoReparacion = 3,
            fabricante = fabricantes[1]
        },
        new Herramienta
        {
            Id = 3,
            Nombre = "Sierra",
            Material = "Madera",
            Precio = 75.0f,
            TiempoReparacion = 4,
            fabricante = fabricantes[2]
        },
        new Herramienta
        {
            Id = 4,
            Nombre = "Destornillador",
            Material = "Metal",
            Precio = 30.0f,
            TiempoReparacion = 2,
            fabricante = fabricantes[0]
        }
    };

            // Crear usuario - AÑADIR correoelectronico
            var user = new ApplicationUser
            {
                Id = "U001",
                Nombre = _customerName,
                Apellido = _customerSurname,
                PhoneNumber = _phoneNumber,
                UserName = _userName,
                Email = _userName, // Para Identity
                correoelectronico = _userName, // Tu campo personalizado
                NormalizedUserName = _userName.ToUpper(),
                NormalizedEmail = _userName.ToUpper(),
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEHASHASHASHASHAS==",
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            // Crear reparación existente (para probar herramientas en reparación)
            var reparacionExistente = new Reparacion
            {
                Id = 1,
                FechaEntrega = DateTime.Now.AddDays(7),
                FechaRecogida = DateTime.Now.AddDays(10),
                PrecioTotal = 200.0f,
                MetodoPago = tiposMetodoPago.Efectivo,
                Cliente = user
            };

            var reparacionItem = new ReparacionItem
            {
                reparacionId = 1,
                herramientaId = 1,
                Herramienta = herramientas[0],
                Cantidad = 1,
                Precio = 10.0f,
                Descripcion = "Reparación de Taladro"
            };

            reparacionExistente.ItemsReparacion = new List<ReparacionItem> { reparacionItem };

            // Guardar datos en la base de datos en memoria
            _context.Fabricante.AddRange(fabricantes);
            _context.Herramienta.AddRange(herramientas);
            _context.ApplicationUser.Add(user);
            _context.Reparacion.Add(reparacionExistente);
            _context.ReparacionItem.Add(reparacionItem);

            _context.SaveChanges();
        }


        

        public static IEnumerable<object[]> TestCasesFor_CreateReparacion()
        {
            // Caso 1: Sin items de reparación
            var reparacionSinItems = new ReparacionDTO
            {
                UserName = _userName,
                NombreCliente = _customerName,
                ApellidosCliente = _customerSurname,
                NumTelefono = _phoneNumber,
                FechaEntrega = DateTime.Today.AddDays(1),
                MetodoPago = tiposMetodoPago.TarjetaCredito,
                ItemsReparacion = new List<ReparacionItemDTO>()
            };

            // Caso 2: Fecha de entrega anterior a hoy
            var reparacionFechaAnterior = new ReparacionDTO
            {
                UserName = _userName,
                NombreCliente = _customerName,
                ApellidosCliente = _customerSurname,
                NumTelefono = _phoneNumber,
                FechaEntrega = DateTime.Today.AddDays(-1),
                MetodoPago = tiposMetodoPago.TarjetaCredito,
                ItemsReparacion = new List<ReparacionItemDTO>
                {
                    new ReparacionItemDTO(2, 1, "Reparación de martillo")
                }
            };

            //Caso examen
            // Errornumero detelefono
            var reparacionNumTelefono = new ReparacionDTO
            {
                UserName = _userName,
                NombreCliente = _customerName,
                ApellidosCliente = _customerSurname,
                NumTelefono = "60033344",
                FechaEntrega = DateTime.Today.AddDays(1),
                MetodoPago = tiposMetodoPago.TarjetaCredito,
                ItemsReparacion = new List<ReparacionItemDTO>
                {
                    new ReparacionItemDTO(2, 1, "Reparación de martillo")
                }
            };

            // Caso 3: Cantidad menor o igual a 0
            var reparacionCantidadInvalida = new ReparacionDTO
            {
                UserName = _userName,
                NombreCliente = _customerName,
                ApellidosCliente = _customerSurname,
                NumTelefono = _phoneNumber,
                FechaEntrega = DateTime.Today.AddDays(1),
                MetodoPago = tiposMetodoPago.TarjetaCredito,
                ItemsReparacion = new List<ReparacionItemDTO>
                {
                    new ReparacionItemDTO(2, 0, "Reparación de martillo")
                }
            };

            // Caso 4: Usuario no encontrado
            var reparacionUsuarioNoEncontrado = new ReparacionDTO
            {
                UserName = "usuarioinexistente@alu",
                NombreCliente = _customerName,
                ApellidosCliente = _customerSurname,
                NumTelefono = _phoneNumber,
                FechaEntrega = DateTime.Today.AddDays(1),
                MetodoPago = tiposMetodoPago.TarjetaCredito,
                ItemsReparacion = new List<ReparacionItemDTO>
                {
                    new ReparacionItemDTO(2, 1, "Reparación de martillo")
                }
            };

            // Caso 5: Herramienta no existe
            var reparacionHerramientaNoExiste = new ReparacionDTO
            {
                UserName = _userName,
                NombreCliente = _customerName,
                ApellidosCliente = _customerSurname,
                NumTelefono = _phoneNumber,
                FechaEntrega = DateTime.Today.AddDays(1),
                MetodoPago = tiposMetodoPago.TarjetaCredito,
                ItemsReparacion = new List<ReparacionItemDTO>
                {
                    new ReparacionItemDTO(999, 1, "Herramienta inexistente")
                }
            };

            // Caso 6: Herramienta ya en reparación
            var reparacionHerramientaEnReparacion = new ReparacionDTO
            {
                UserName = _userName,
                NombreCliente = _customerName,
                ApellidosCliente = _customerSurname,
                NumTelefono = _phoneNumber,
                FechaEntrega = DateTime.Today.AddDays(1),
                MetodoPago = tiposMetodoPago.TarjetaCredito,
                ItemsReparacion = new List<ReparacionItemDTO>
                {
                    new ReparacionItemDTO(1, 1, "Taladro ya en reparación")
                }
            };

            var allTests = new List<object[]>
            {
                new object[] { reparacionSinItems, "Error! Debe incluir al menos una herramienta para reparar" },
                new object[] { reparacionFechaAnterior, "Error! La fecha de entrega debe ser posterior a hoy" },
                new object[] { reparacionNumTelefono, "Error!, el telefono debe empezar por +34" },
                new object[] { reparacionCantidadInvalida, "Error! La cantidad de todas las herramientas debe ser al menos 1" },
                new object[] { reparacionUsuarioNoEncontrado, "Error! Usuario no autenticado o no encontrado" },
                new object[] { reparacionHerramientaNoExiste, "Error! La herramienta con ese ID no existe" },
                new object[] { reparacionHerramientaEnReparacion, "Error! La herramienta con ese nombre no está disponible para reparación" }
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateReparacion))]
        public async Task CreateReparacion_Error_test(ReparacionDTO reparacionDTO, string errorExpected)
        {

            // Arrange
            var mockLogger = new Mock<ILogger<ReparacionController>>();
            var controller = new ReparacionController(_context, mockLogger.Object);
            // Act
            var result = await controller.CreateReparacion(reparacionDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            // Obtener todos los errores
            var todosLosErrores = problemDetails.Errors
                .SelectMany(error => error.Value)
                .ToList();

            // Debug: mostrar errores si el test falla
            var errorActual = todosLosErrores.FirstOrDefault(error => error.Contains(errorExpected));

            if (errorActual == null)
            {
                // Si no encuentra el error, mostrar qué errores sí hay
                var erroresDisponibles = string.Join("; ", todosLosErrores);
                Assert.True(false, $"Error esperado no encontrado. Esperado: '{errorExpected}'. Encontrados: {erroresDisponibles}");
            }
            Assert.Contains(errorExpected, errorActual);
            Assert.Equal(errorExpected, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateReparacion_Success_test()
        {
            // Arrange
            var reparacionDTO = new ReparacionDTO
            {
                UserName = _userName,
                NombreCliente = _customerName,
                ApellidosCliente = _customerSurname,
                NumTelefono = _phoneNumber,
                FechaEntrega = DateTime.Today.AddDays(1),
                MetodoPago = tiposMetodoPago.PayPal,
                ItemsReparacion = new List<ReparacionItemDTO>
                {
                    new ReparacionItemDTO(2, 2, "Reparación de martillos"),
                    new ReparacionItemDTO(3, 1, "Reparación de sierra")
                }
            };

            // Calcular fechas esperadas
            var fechaEntregaEsperada = DateTime.Today.AddDays(1);
            var maxTiempoReparacion = 4; // Sierra tiene 4 días
            var fechaRecogidaEsperada = CalcularFechaRecogida(fechaEntregaEsperada, maxTiempoReparacion);

            // Precio total esperado: (50 * 2) + (75 * 1) = 175
            var precioTotalEsperado = 175.0f;

            // Arrange
            var mockLogger = new Mock<ILogger<ReparacionController>>();
            var controller = new ReparacionController(_context, mockLogger.Object);

            // Act
            var result = await controller.CreateReparacion(reparacionDTO);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var reparacionDetailDTO = Assert.IsType<ReparacionDetailsDTO>(createdResult.Value);

            // Verificar datos básicos
            Assert.Equal(_customerName, reparacionDetailDTO.NombreCliente);
            Assert.Equal(_customerSurname, reparacionDetailDTO.ApellidosCliente);
            Assert.Equal(_phoneNumber, reparacionDetailDTO.NumTelefono);
            Assert.Equal(fechaEntregaEsperada.Date, reparacionDetailDTO.FechaEntrega.Date);
            Assert.Equal(fechaRecogidaEsperada.Date, reparacionDetailDTO.FechaRecogida.Date);
            Assert.Equal(precioTotalEsperado, reparacionDetailDTO.PrecioTotal);
            Assert.Equal(tiposMetodoPago.PayPal, reparacionDetailDTO.MetodoPago);

            // Verificar items
            Assert.Equal(2, reparacionDetailDTO.ItemsReparacion.Count);

            var itemMartillo = reparacionDetailDTO.ItemsReparacion.FirstOrDefault(i => i.NombreHerramienta == "Martillo");
            Assert.NotNull(itemMartillo);
            Assert.Equal(2, itemMartillo.Cantidad);
            Assert.Equal(100.0f, itemMartillo.Precio); // 50 * 2 = 100
            Assert.Equal("Reparación de martillos", itemMartillo.Descripcion);

            var itemSierra = reparacionDetailDTO.ItemsReparacion.FirstOrDefault(i => i.NombreHerramienta == "Sierra");
            Assert.NotNull(itemSierra);
            Assert.Equal(1, itemSierra.Cantidad);
            Assert.Equal(75.0f, itemSierra.Precio);
            Assert.Equal("Reparación de sierra", itemSierra.Descripcion);

            Assert.Equal(itemMartillo, reparacionDetailDTO.ItemsReparacion.FirstOrDefault(i => i.NombreHerramienta == "Martillo"));
            Assert.Equal(itemSierra, reparacionDetailDTO.ItemsReparacion.FirstOrDefault(i => i.NombreHerramienta == "Sierra"));
        }

        private DateTime CalcularFechaRecogida(DateTime fechaEntrega, int diasHabiles)
        {
            DateTime fechaRecogida = fechaEntrega;
            int diasAgregados = 0;
            while (diasAgregados < diasHabiles)
            {
                fechaRecogida = fechaRecogida.AddDays(1);
                if (fechaRecogida.DayOfWeek != DayOfWeek.Saturday && fechaRecogida.DayOfWeek != DayOfWeek.Sunday)
                {
                    diasAgregados++;
                }
            }
            return fechaRecogida;
        }

    }
}

