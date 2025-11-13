using AppForSEII2526.API;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.Alquilar_Herramienta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.HerramientasController_test
{
    public class GetAlquileresDisponibles_test : AppForSEII25264SqliteUT
    {
        public GetAlquileresDisponibles_test()
        {
            var fabricante = new List<Fabricante>()
            {
                new Fabricante() { Id = 1, Nombre = "Fabricante1" },
                new Fabricante() { Id = 2, Nombre = "Fabricante2" },
                new Fabricante() { Id = 3, Nombre = "Fabricante3" },
            };
            var herramienta = new List<Herramienta>()
            {
                new Herramienta() { Id = 1, Nombre = "Taladro", Material = "Acero", Precio = 100.0f, fabricante = fabricante[0] },
                new Herramienta() { Id = 2, Nombre = "Destornillador", Material = "Metal", Precio = 50.0f, fabricante = fabricante[1] },
                new Herramienta() { Id = 3, Nombre = "Motosierra", Material = "Metal", Precio = 75.0f, fabricante = fabricante[0] },
                new Herramienta() { Id = 4, Nombre = "Martillo", Material = "Madera", Precio = 30.0f, fabricante = fabricante[2] },
            };
            _context.Herramienta.AddRange(herramienta);
            _context.SaveChanges();
        }
        public static IEnumerable<object[]> TestCasesFor_GetAlquileresDisponibles_OK()
        {
            var fabricante = new List<Fabricante>()
            {
                new Fabricante() { Id = 1, Nombre = "Fabricante1" },
                new Fabricante() { Id = 2, Nombre = "Fabricante2" },
                new Fabricante() { Id = 3, Nombre = "Fabricante3" },
            };
            var alquilerHerramientasDTO = new List<AlquilerHerramientasDTO>()
            {
                new AlquilerHerramientasDTO(1, "Taladro", "Acero", 100.0f, fabricante[0]),
                new AlquilerHerramientasDTO(2, "Destornillador", "Metal", 50.0f, fabricante[1]),
                new AlquilerHerramientasDTO(3, "Motosierra", "Metal", 75.0f, fabricante[0]),
                new AlquilerHerramientasDTO(4, "Martillo", "Madera", 30.0f, fabricante[2])
            };
            fabricante[0].Herramientas = new List<Herramienta>()
            {
                new Herramienta() { Id = 1, Nombre = "Taladro", Material = "Acero", Precio = 100.0f, fabricante = fabricante[0] },
                new Herramienta() { Id = 3, Nombre = "Motosierra", Material = "Metal", Precio = 75.0f, fabricante = fabricante[0] },
            };
            fabricante[1].Herramientas = new List<Herramienta>()
            {
                new Herramienta() { Id = 2, Nombre = "Destornillador", Material = "Metal", Precio = 50.0f, fabricante = fabricante[1] },
            };
            fabricante[2].Herramientas = new List<Herramienta>()
            {
                new Herramienta() { Id = 4, Nombre = "Martillo", Material = "Madera", Precio = 30.0f, fabricante = fabricante[2] },
            };
            var alquilerHerramientasDTOsTC1 = new List<AlquilerHerramientasDTO> { alquilerHerramientasDTO[0], alquilerHerramientasDTO[1], alquilerHerramientasDTO[2], alquilerHerramientasDTO[3] }.OrderBy(h => h.Id).ToList();
            var alquilerHerramientasDTOsTC2 = new List<AlquilerHerramientasDTO> { alquilerHerramientasDTO[0] };
            var alquilerHerramientasDTOsTC3 = new List<AlquilerHerramientasDTO> { alquilerHerramientasDTO[1], alquilerHerramientasDTO[2] }.OrderBy(h => h.Id).ToList();
            var alquilerHerramientasDTOsTC4 = new List<AlquilerHerramientasDTO> { alquilerHerramientasDTO[1] };
            var allTests = new List<object[]>()
            {
                // Sin filtros
                new object[] { null, null , alquilerHerramientasDTOsTC1 },
                // Filtrado por nombre
                new object[] { "Taladro", null , alquilerHerramientasDTOsTC2},
                // Filtrado por material
                new object[] { null, "Metal" , alquilerHerramientasDTOsTC3},
                // Filtrado por nombre y material
                new object[] { "Destornillador", "Metal" , alquilerHerramientasDTOsTC4}
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetAlquileresDisponibles_OK))]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetAlquileresDisponibles_OK_test(string? nombre, string? material, IList<AlquilerHerramientasDTO> herramientasEsperadas)
        {
            // Arrange
            var controller = new HerramientasController(_context, null);
            // Act
            var result = await controller.GetAlquileresDisponibles(nombre, material);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var alquilarHerramientasDTOs = Assert.IsType<List<AlquilerHerramientasDTO>>(okResult.Value);
            Assert.Equal(herramientasEsperadas, alquilarHerramientasDTOs);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetAlquileresDisponibles_EmptyResult_test()
        {
            // Arrange
            var mock = new Mock<ILogger<HerramientasController>>();
            ILogger<HerramientasController> logger = mock.Object;
            var controller = new HerramientasController(_context, logger);
            // Act
            var result = await controller.GetAlquileresDisponibles("Cascos", "PVC");
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(notFoundResult.Value);
            var problem = problemDetails.Errors.First().Value[0];

            Assert.Equal("No hay herramientas disponibles con los parámetros introducidos.", problem);
        }
    }
}
