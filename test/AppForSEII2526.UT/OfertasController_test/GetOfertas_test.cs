using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.OfertaDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.OfertasController_test
{
    public class GetOfertas_test : AppForSEII25264SqliteUT
    {

        public GetOfertas_test()
        {
            /******************************************** 
            * Constructor:
            * Crear datos de prueba en la base de datos en memoria
            * 
            *******************************************/


            Fabricante fabricante =
                new Fabricante
                {
                    Id = 1,
                    Nombre = "Fabricante1"
                };

            Herramienta herramienta =
                new Herramienta
                {
                    Id = 1,
                    Nombre = "Lija",
                    Material = "Cuero",
                    Precio = 100,
                    fabricante = fabricante
                };

            Oferta oferta = new Oferta
            {
                Id = 1,
                porcentaje = 10,
                fechaInicio = DateTime.Today.AddDays(1),
                fechaFinal = DateTime.Today.AddDays(10),
                fechaOferta = DateTime.Today.AddDays(2),
                metodoPago = tiposMetodoPago.TarjetaCredito,
                dirigidaA = tiposDiridaOferta.Clientes,
                ofertaItems = new List<OfertaItem>
                {
                    new OfertaItem
                    {
                        herramienta = herramienta,
                        herramientaId = herramienta.Id,
                        precioFinal = 90 // 10% de descuento
                    }
                }
            };

            _context.Fabricante.Add(fabricante);
            _context.Herramienta.Add(herramienta);
            _context.Oferta.Add(oferta);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetOfertas_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<OfertasController>>();
            ILogger<OfertasController> logger = mock.Object;

            var controller = new OfertasController(_context, logger);

            // Act
            var result = await controller.GetOferta(999); // ID que no existe

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetOfertas_Found_test()
        {
            // Arrange
            var mock = new Mock<ILogger<OfertasController>>();
            ILogger<OfertasController> logger = mock.Object;
            var controller = new OfertasController(_context, logger);

            var expectedOferrta = new OfertaDetailsDTO(
                1,
                10,
                DateTime.Today.AddDays(1),
                DateTime.Today.AddDays(10),
                DateTime.Today.AddDays(2),
                tiposMetodoPago.TarjetaCredito,
                tiposDiridaOferta.Clientes,
                new List<OfertaItemDTO>
                {
                    new OfertaItemDTO(
                        1,
                        90,
                        "Lija",
                        "Cuero",
                        "Fabricante1",
                        100
                    )
                }
            );


            // Act
            var result = await controller.GetOferta(1); // ID que existe

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualOferrta = Assert.IsType<OfertaDetailsDTO>(okResult.Value);

                // Comprobar que el real y el esperado son iguales
            Assert.Equal(expectedOferrta, actualOferrta);
        }
    }
}
