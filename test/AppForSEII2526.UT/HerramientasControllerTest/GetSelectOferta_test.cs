using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.OfertaDTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.HerramientasControllerTest
{
    public class GetSelectOferta_test : AppForSEII25264SqliteUT
    {
        public GetSelectOferta_test()
        {
            var fabricantes = new List<Fabricante>()
            {
                new Fabricante() { Id=1, Nombre="Fabricante1"},
                new Fabricante() { Id=2, Nombre="Fabricante2"},
            };

            var herramientas = new List<Herramienta>()
            {
                new Herramienta() { Id=1, Nombre="Herramienta1", Material="Madera", Precio=100, fabricante=fabricantes[0]},
                new Herramienta() { Id=2, Nombre="Herramienta2", Material="Metal", Precio=200, fabricante=fabricantes[1]},
                new Herramienta() { Id=3, Nombre="Herramienta3", Material="Plastico", Precio=300, fabricante=fabricantes[0]},
            };

            _context.Fabricante.AddRange(fabricantes);
            _context.Herramienta.AddRange(herramientas);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesDor_GetSelectOfertaData_OK()
        {
            var fabricantes = new List<API.Models.Fabricante>()
            {
                new Fabricante() { Id=1, Nombre="Fabricante1"},
                new Fabricante() { Id=2, Nombre="Fabricante2"},
            };

            var herramientaForOfertaDTOs = new List<HerramientaForOfertaDTO>()
            {
                new HerramientaForOfertaDTO(1, "Herramienta1", "Madera", 100, fabricantes[0]),
                new HerramientaForOfertaDTO(2, "Herramienta2", "Metal", 200, fabricantes[1]),
                new HerramientaForOfertaDTO(3, "Herramienta3", "Plastico", 300, fabricantes[0]),
            };

            // Relacionar los fabricantes a las herramientas para no recibir errores en las pruebas
            fabricantes[0].Herramientas = new List<Herramienta>()
            {
                new Herramienta() { Id=1, Nombre="Herramienta1", Material="Madera", Precio=100, fabricante=fabricantes[0]},
                new Herramienta() { Id=3, Nombre="Herramienta3", Material="Plastico", Precio=300, fabricante=fabricantes[0]},
            };
            fabricantes[1].Herramientas = new List<Herramienta>()
            {
                new Herramienta() { Id=2, Nombre="Herramienta2", Material="Metal", Precio=200, fabricante=fabricantes[1]},
            };

            var herramientaDTOsTC1 = new List<HerramientaForOfertaDTO>()
            {
                herramientaForOfertaDTOs[0],
                herramientaForOfertaDTOs[1],
                herramientaForOfertaDTOs[2],
            };

            var herramientaDTOsTC2 = new List<HerramientaForOfertaDTO>()
            {
                herramientaForOfertaDTOs[1],
            };

            var herramientaDTOsTC3 = new List<HerramientaForOfertaDTO>()
            {
                herramientaForOfertaDTOs[0],
                herramientaForOfertaDTOs[1]
            };

            var allTests = new List<object[]>()
            {
                // Sin filtros
                new object[] { null, 0f, herramientaDTOsTC1 },
                // Filtrar por fabricante2
                new object[] { "Fabricante2", 0f, herramientaDTOsTC2 },
                // Filtrar por precio máximo 200
                new object[] { null, 200f, herramientaDTOsTC3 }
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesDor_GetSelectOfertaData_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetHerramientasForOferta_OK_Test(string? fabricante, float? precio,
            IList<HerramientaForOfertaDTO> expectedHerramientas)
        {
            //Arrange
            var controller = new HerramientasController(_context, null);

            //Act
            var result = await controller.GetHerramientaForOferta(fabricante, precio);

            //Asert
            //Comprobar que la respuesta type es OK
            var okResult = Assert.IsType<OkObjectResult>(result);
            //Y obtener la lista de películas
            var herramientaDTOsactual = Assert.IsType<List<HerramientaForOfertaDTO>>(okResult.Value);
            Assert.Equal(expectedHerramientas, herramientaDTOsactual);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetHerramientasForOferta_badrequest_test()
        {
            // Arrange
            var mock = new Mock<ILogger<HerramientasController>>();
            ILogger<HerramientasController> logger = mock.Object;
            var controller = new HerramientasController(_context, logger);

            //Act
            var result = await controller.GetHerramientaForOferta("FabricanteNoExistente", -50f);

            //Asert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var problem = problemDetails.Errors.First().Value[0];

            Assert.Equal("El precio no puede ser negativo", problem);
        }
    }
    }
