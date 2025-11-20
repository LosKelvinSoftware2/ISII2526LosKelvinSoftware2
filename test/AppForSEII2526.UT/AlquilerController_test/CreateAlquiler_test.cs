using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.Alquilar_Herramienta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.AlquilerController_test
{
    public class CreateAlquiler_test : AppForSEII25264SqliteUT
    {
        public CreateAlquiler_test()
        {
            var fabricantePrueba = new Fabricante()
            {
                Id = 1,
                Nombre = "Bosch",
            };
            var herramientas = new List<Herramienta>()
            {
                new Herramienta { Id = 1, Nombre = "Taladro", Material = "Acero", Precio = 100.0f , fabricante = fabricantePrueba},
                new Herramienta { Id = 2, Nombre = "Destornillador", Material = "Metal", Precio = 50.0f ,fabricante = fabricantePrueba}
            };

            _context.Fabricante.Add(fabricantePrueba);
            _context.Herramienta.AddRange(herramientas);

            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreateAlquiler()
        {
            var fabricantePrueba = new Fabricante()
            {
                Id = 1,
                Nombre = "Bosch",
            };
            var herramientas = new List<Herramienta>()
            {
                new Herramienta { Id = 1, Nombre = "Taladro", Material = "Acero", Precio = 100.0f , fabricante = fabricantePrueba},
                new Herramienta { Id = 2, Nombre = "Destornillador", Material = "Metal", Precio = 50.0f ,fabricante = fabricantePrueba}
            };
            var alquilarItemsDTO = new List<AlquilarItemDTO>()
            {
                new AlquilarItemDTO(herramientas[0].Nombre , herramientas[0].Material , 1 , 100.0f),
                new AlquilarItemDTO(herramientas[1].Nombre , herramientas[1].Material , 2 , 50.0f)
            };
            var cliente = new ApplicationUser()
            {
                Id = "user1",
                Nombre = "Juan",
                Apellido = "Pérez",
                correoelectronico = "juanperez@gmail.com"
            };
            var alquilerAntesHoy = new AlquilerDTO(cliente.Nombre, cliente.Apellido , "Calle falsa 123", 150.0f, DateTime.Today.AddDays(5), DateTime.Today.AddDays(-1), alquilarItemsDTO, tiposMetodoPago.Efectivo);
            var alquilerFechaFinMenorFechaInicio = new AlquilerDTO(cliente.Nombre, cliente.Apellido, "Calle falsa 123", 150.0f, DateTime.Today.AddDays(3), DateTime.Today.AddDays(5), alquilarItemsDTO, tiposMetodoPago.Efectivo);
            var alquilerSinHerramientas = new AlquilerDTO(cliente.Nombre, cliente.Apellido, "Calle falsa 123", 150.0f, DateTime.Today.AddDays(5), DateTime.Today.AddDays(3), new List<AlquilarItemDTO>(), tiposMetodoPago.Efectivo);
            var alquilerSinNombreCliente = new AlquilerDTO("", cliente.Apellido, "Calle falsa 123", 150.0f, DateTime.Today.AddDays(5), DateTime.Today.AddDays(3), alquilarItemsDTO, tiposMetodoPago.Efectivo);
            var alquilerSinApellidoCliente = new AlquilerDTO(cliente.Nombre, "", "Calle falsa 123", 150.0f, DateTime.Today.AddDays(5), DateTime.Today.AddDays(3), alquilarItemsDTO, tiposMetodoPago.Efectivo);
            var alquilerSinDireccionEnvio = new AlquilerDTO(cliente.Nombre, cliente.Apellido,"", 150.0f, DateTime.Today.AddDays(5), DateTime.Today.AddDays(3), alquilarItemsDTO, tiposMetodoPago.Efectivo);
            var alquilerConDireccionEnvioErronea = new AlquilerDTO(cliente.Nombre, cliente.Apellido, "Dirección falsa 123", 150.0f, DateTime.Today.AddDays(5), DateTime.Today.AddDays(3), alquilarItemsDTO, tiposMetodoPago.Efectivo);
            var alquilerSinMetodoPagoValido = new AlquilerDTO(cliente.Nombre, cliente.Apellido, "Calle falsa 123", 150.0f, DateTime.Today.AddDays(5), DateTime.Today.AddDays(3), alquilarItemsDTO, (tiposMetodoPago)999);
            var alquilerCantidadNegativa = new AlquilerDTO(cliente.Nombre, cliente.Apellido, "Calle falsa 123", 150.0f, DateTime.Today.AddDays(5), DateTime.Today.AddDays(3), new List<AlquilarItemDTO>() {
                new AlquilarItemDTO(herramientas[0].Nombre, herramientas[0].Material, -1, 100.0f)
            }, tiposMetodoPago.Efectivo);
            var allReturns = new List<object[]>
            {
                new object[] { alquilerAntesHoy, "El alquiler debe empezar después de hoy" },
                new object[] { alquilerFechaFinMenorFechaInicio, "El alquiler debe terminar después de iniciar" },
                new object[] { alquilerSinHerramientas, "Debe haber al menos una herramienta a alquilar" },
                new object[] { alquilerSinNombreCliente, "El nombre es obligatorio" },
                new object[] { alquilerSinApellidoCliente, "El apellido es obligatorio" },
                new object[] { alquilerSinDireccionEnvio, "La dirección de envío es obligatoria" },
                new object[] { alquilerConDireccionEnvioErronea, "¡Error! La dirección de envío debe empezar por la palabra Calle"},
                new object[] { alquilerSinMetodoPagoValido, "El método de pago es obligatorio" },
                new object[] { alquilerCantidadNegativa, "Debe especificarse una cantidad válida para cada herramienta" },
            };
            return allReturns;
        }
        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateAlquiler))]
        public async Task CreateAlquiler_Error_test(AlquilerDTO alquilerForCreate, string expectedErrorMessage)
        {
            // Arrange
            var mock = new Mock<ILogger<AlquilerController>>();
            ILogger<AlquilerController> logger = mock.Object;
            var controller = new AlquilerController(_context, logger);
            // Act
            var result = await controller.CreateAlquiler(alquilerForCreate);
            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            Assert.StartsWith(expectedErrorMessage, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateAlquiler_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<AlquilerController>>();
            ILogger<AlquilerController> logger = mock.Object;
            var controller = new AlquilerController(_context, logger);
            var fabricantePrueba = new Fabricante()
            {
                Id = 1,
                Nombre = "Bosch",
            };
            var herramientas = new List<Herramienta>()
            {
                new Herramienta { Id = 1, Nombre = "Taladro", Material = "Acero", Precio = 100.0f , fabricante = fabricantePrueba},
                new Herramienta { Id = 2, Nombre = "Destornillador", Material = "Metal", Precio = 50.0f ,fabricante = fabricantePrueba}
            };
            var alquilarItemsDTO = new List<AlquilarItemDTO>()
            {
                new AlquilarItemDTO(herramientas[0].Nombre, herramientas[0].Material , 1 , 100.0f),
                new AlquilarItemDTO(herramientas[1].Nombre, herramientas[1].Material , 2 , 50.0f)
            };
            var cliente = new ApplicationUser()
            {
                Id = "user1",
                Nombre = "Juan",
                Apellido = "Pérez",
                correoelectronico = "juanperez@gmail.com"

            };
            var expectedAlquilerDetailDTO = new AlquilerDetailDTO(1 , DateTime.Today, cliente.Nombre, cliente.Apellido, "Calle falsa 123", 200.0f, DateTime.Today.AddDays(10), DateTime.Today.AddDays(5), alquilarItemsDTO, tiposMetodoPago.Efectivo);
            var alquilerToCreate = new AlquilerDTO(cliente.Nombre, cliente.Apellido, "Calle falsa 123", 200.0f, DateTime.Today.AddDays(10), DateTime.Today.AddDays(5), alquilarItemsDTO, tiposMetodoPago.Efectivo);
            // Act
            var result = await controller.CreateAlquiler(alquilerToCreate);
            //Assert
            var okResult = Assert.IsType<CreatedAtActionResult>(result);
            var alquilerDetailDTO = Assert.IsType<AlquilerDetailDTO>(okResult.Value);
            Assert.Equal(expectedAlquilerDetailDTO, alquilerDetailDTO);

        }
    }
}
