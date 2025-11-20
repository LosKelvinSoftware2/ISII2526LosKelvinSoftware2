using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.OfertaDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.OfertasController_test
{
    public class PostOferta_test : AppForSEII25264SqliteUT
    {
        private const string herramientaNombre1 = "Taladro";
        private const string nombreFabricante1 = "Bosch";
        private const string herramientaNombre2 = "Sierra";
        private const string nombreFabricante2 = "Black&Decker";

        public PostOferta_test()
        {
            var fabricantes = new List<Fabricante>()
            {
                new Fabricante
                {
                    Id = 1,
                    Nombre = nombreFabricante1,
                },
                new Fabricante
                {
                    Id = 2,
                    Nombre = nombreFabricante2,
                }
            };

            var herramientas = new List<Herramienta>()
            {
                new Herramienta
                {
                    Id = 1,
                    Nombre = herramientaNombre1,
                    Material = "Metal",
                    fabricante = fabricantes[0]
                },
                new Herramienta
                {
                    Id = 2,
                    Nombre = herramientaNombre2,
                    Material = "Madera",
                    fabricante = fabricantes[1]
                }
            };

            var oferta = new Oferta
            {
                Id = 1,
                porcentaje = 15,
                fechaInicio = DateTime.Today,
                fechaFinal = DateTime.Today.AddMonths(1),
                fechaOferta = DateTime.Today.AddDays(1),
                metodoPago = tiposMetodoPago.TarjetaCredito,
                dirigidaA = tiposDiridaOferta.Clientes,
                ofertaItems = new List<OfertaItem>()
                {
                    new OfertaItem
                    {
                        ofertaId = 1,
                        herramienta = herramientas[0],
                        porcentaje = 15,
                        precioFinal = 85
                    },
                }
            };

            _context.Fabricante.AddRange(fabricantes);
            _context.Herramienta.AddRange(herramientas);
            _context.Oferta.Add(oferta);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreateOferta()
        {

            var ofertaItems = new List<OfertaItemDTO>(){
                new OfertaItemDTO(1, 90, "Taladro", "Metal", "Bosch", 100),
                new OfertaItemDTO(2, 170, "Sierra", "Madera", "Black&Decker", 200)
            };

            var ofertaFromBeforeToday = new OfertaDTO
            (
                10,
                DateTime.Today.AddMonths(1),
                DateTime.Today.AddDays(-1),
                tiposMetodoPago.Efectivo,
                tiposDiridaOferta.Clientes,
                ofertaItems
            );

            var ofertaoBeforeFrom = new OfertaDTO
            (
                10,
                DateTime.Today,
                DateTime.Today.AddMonths(1),
                tiposMetodoPago.Efectivo,
                tiposDiridaOferta.Clientes,
                ofertaItems
            );

            // El nuevo caso para el examen
            var ofertaLessThanWeek = new OfertaDTO
            (
                10,
                DateTime.Today.AddDays(5),
                DateTime.Today,
                tiposMetodoPago.Efectivo,
                tiposDiridaOferta.Clientes,
                ofertaItems
            );

            var ofertaWithInvalidPercentage = new OfertaDTO
            (
                0,
                DateTime.Today.AddMonths(1),
                DateTime.Today.AddDays(1),
                tiposMetodoPago.Efectivo,
                tiposDiridaOferta.Clientes,
                ofertaItems
            );

            var ofertaNoItems = new OfertaDTO
            (
                10,
                DateTime.Today.AddMonths(1),
                DateTime.Today.AddDays(1),
                tiposMetodoPago.Efectivo,
                tiposDiridaOferta.Clientes,
                new List<OfertaItemDTO>()
            );

            var allTests = new List<object[]>
            {
                new object[] { ofertaFromBeforeToday, "Error! La fecha de inicio de oferta debe ser al menos mañana" },
                new object[] { ofertaoBeforeFrom, "Error! La fecha final de oferta debe ser después de la fecha de inicio" },
                // Las nuevas entradas (el object añadido)
                new object[] {ofertaLessThanWeek, "Error!, la oferta debe durar al menos una semana" },
                new object[] { ofertaWithInvalidPercentage, "Error! El porcentaje de descuento debe estar entre 1 y 100" },
                new object[] { ofertaNoItems, "Error! Hay que incluir al menos una herramienta" }
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateOferta))]
        public async Task CreateOferta_Error_test(OfertaDTO ofertaForCreate, string expectedErrorMessage)
        {
            // Arrange
            var mock = new Mock<ILogger<OfertasController>>();
            ILogger<OfertasController> logger = mock.Object;

            var controller = new OfertasController(_context, logger);

            // Act
            var result = await controller.CreateOferta(ofertaForCreate);
            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var ProblemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorReal = ProblemDetails.Errors.First().Value[0];

            Assert.StartsWith(expectedErrorMessage, errorReal);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateOferta_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<OfertasController>>();
            ILogger<OfertasController> logger = mock.Object;

            var controller = new OfertasController(_context, logger);
            var ofertaItems = new List<OfertaItemDTO>(){
                new OfertaItemDTO(1, 90, "Taladro", "Metal", "Bosch", 100),
                new OfertaItemDTO(2, 170, "Sierra", "Madera", "Black&Decker", 200)
            };
            var ofertaForCreate = new OfertaDTO
            (
                10,
                DateTime.Today.AddMonths(1),
                DateTime.Today.AddDays(1),
                tiposMetodoPago.Efectivo,
                tiposDiridaOferta.Clientes,
                ofertaItems
            );
            var expectedOferta = new OfertaDetailsDTO
            (
                2, // ID esperado de la nueva oferta
                10,
                DateTime.Today.AddDays(1),
                DateTime.Today.AddMonths(1),
                DateTime.Today,
                tiposMetodoPago.Efectivo,
                tiposDiridaOferta.Clientes,
                ofertaItems
            );

            // Act
            var result = await controller.CreateOferta(ofertaForCreate);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdOferta = Assert.IsType<OfertaDetailsDTO>(createdAtActionResult.Value);

            Assert.Equal(expectedOferta, createdOferta);
        }

    }
}
