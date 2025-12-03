using AppForSEII2526.API;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.OfertaDTOs;
using AppForSEII2526.API.Models;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.OfertasController_test
{
    public class GetOferta_test : AppForSEII25264SqliteUT
    {
        public GetOferta_test()
        {
            var fabricante = new Fabricante
            {
                Id = 1,
                Nombre = "Bosch",
            };

            var herramienta = new Herramienta
            {
                Id = 1,
                Nombre = "Taladro",
                Material = "Metal",
                fabricante = fabricante,
                Precio = 100
            };

            var ofertaItems = new List<OfertaItem>()
            {
                new OfertaItem
                    {
                        ofertaId = 1,
                        herramienta = herramienta,
                        porcentaje = 10,
                        precioFinal = 90
                    }
            };

            var oferta = new Oferta
            {
                Id = 1,
                porcentaje = 10,
                fechaInicio = DateTime.Today,
                fechaFinal = DateTime.Today.AddMonths(1),
                fechaOferta = DateTime.Today.AddDays(1),
                ofertaItems = ofertaItems,
                MetodoPago = tiposMetodoPago.TarjetaCredito
            };
            

            _context.Fabricante.Add(fabricante);
            _context.Herramienta.Add(herramienta);
            _context.OfertaItem.AddRange(ofertaItems);
            _context.Oferta.Add(oferta);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetRental_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<OfertasController>>();
            ILogger<OfertasController> logger = mock.Object;

            var controller = new OfertasController(_context, logger);

            // Act
            var result = await controller.GetOferta(0);

            //Assert
            //we check that the response type is OK and obtain the list of movies
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetRental_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<OfertasController>>();
            ILogger<OfertasController> logger = mock.Object;
            var controller = new OfertasController(_context, logger);


            DateTime Unspec(DateTime d) => DateTime.SpecifyKind(d, DateTimeKind.Unspecified);

            var expectedOferta = new OfertaDetailsDTO(
                Unspec(DateTime.Today),
                Unspec(DateTime.Today.AddMonths(1)),
                Unspec(DateTime.Today.AddDays(1)),
                tiposMetodoPago.TarjetaCredito,
                null,
                new List<OfertaItemDTO>()
            );
            expectedOferta.ofertaItems.Add(
                new OfertaItemDTO(
                    10,
                    90,
                    "Taladro",
                    "Metal",
                    "Bosch",
                    100
                )
            );

            // Act
            var result = await controller.GetOferta(1);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var ofertaDTOReal = Assert.IsType<OfertaDetailsDTO>(okResult.Value);
            var eq = expectedOferta.Equals(ofertaDTOReal);
            // Comprobar que la oferta esperada es igual a la oferta real
            Assert.Equal(expectedOferta, ofertaDTOReal);

        }
    }
}
