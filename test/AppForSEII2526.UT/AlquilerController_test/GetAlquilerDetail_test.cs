using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.Alquilar_Herramienta;
using AppForSEII2526.API.Models;
using Microsoft.OpenApi.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.AlquilerController_test
{
    public class GetAlquilerDetail_test : AppForSEII25264SqliteUT
    {
        public GetAlquilerDetail_test()
        {
            var fabricantePrueba = new Fabricante()
            {
                Id = 1,
                Nombre = "Bosch",
            };
            var cliente = new ApplicationUser()
            {
                Id = "user1",
                Nombre = "Juan",
                Apellido = "Pérez",
                correoelectronico = "juanperez@gmail.com"
            };
            var herramientas = new List<Herramienta>()
            {
                new Herramienta { Id = 1, Nombre = "Taladro", Material = "Acero", Precio = 100.0f , fabricante = fabricantePrueba},
                new Herramienta { Id = 2, Nombre = "Destornillador", Material = "Metal", Precio = 50.0f ,fabricante = fabricantePrueba}
            };
            var alquilarItems = new List<AlquilarItem>()
                {
                    new AlquilarItem()
                    {
                        alquilerId = 1,
                        herramienta = herramientas[0],
                        cantidad = 1,
                        precio = 100.0f
                    },
                    new AlquilarItem()
                    {
                        alquilerId = 1,
                        herramienta = herramientas[1],
                        cantidad = 2,
                        precio = 50.0f
                    }
                };

            var alquilerPrueba = new Alquiler()
            {
                Id = 1,
                fechaInicio = new DateTime(2023, 1, 1),
                fechaFin = new DateTime(2023, 1, 10),
                fechaAlquiler = new DateTime(2023, 1, 1),
                Cliente = cliente,
                direccionEnvio = "Calle Falsa 123",
                precioTotal = 150.0f,
                MetodoPago = tiposMetodoPago.Efectivo,
                AlquilarItems = alquilarItems
            };

            


            _context.ApplicationUser.Add(cliente);
            _context.Herramienta.AddRange(herramientas);
            _context.AlquilarItem.AddRange(alquilarItems);
            _context.Alquiler.Add(alquilerPrueba);
            _context.SaveChanges();

        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetAlquilerDetail_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<AlquilerController>>();
            ILogger<AlquilerController> logger = mock.Object;
            var controller = new AlquilerController(_context, logger);
            // Act
            var result = await controller.GetAlquilerDetail(0);  // Comprobamos que no existe alquiler con id 0
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetAlquilerDetail_Found_test()
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
            var cliente = new ApplicationUser()
            {
                Id = "user1",
                Nombre = "Juan",
                Apellido = "Pérez",
                correoelectronico = "juanperez@gmail.com"
            };
            var herramientas = new List<Herramienta>()
            {
                new Herramienta { Id = 1, Nombre = "Taladro", Material = "Acero", Precio = 100.0f , fabricante = fabricantePrueba},
                new Herramienta { Id = 2, Nombre = "Destornillador", Material = "Metal", Precio = 50.0f , fabricante = fabricantePrueba }
            };
            var alquilarItems = new List<AlquilarItemDTO>()
            {
                new AlquilarItemDTO(herramientas[0].Nombre, herramientas[0].Material , 1 , 100.0f),
                new AlquilarItemDTO(herramientas[1].Nombre, herramientas[1].Material , 2 , 50.0f)
            };

            var expectedAlquilerDetailDTO = new AlquilerDetailDTO(
                 1,
                 new DateTime(2023, 1, 1),
                 cliente.Nombre,
                 cliente.Apellido,
                 "Calle Falsa 123",
                 150.0f,
                 new DateTime(2023, 1, 10),
                 new DateTime(2023, 1, 1),
                 alquilarItems,
                 tiposMetodoPago.Efectivo
             );

            // Act 
            var result = await controller.GetAlquilerDetail(1);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var alquilerDetailDTOActual = Assert.IsType<AlquilerDetailDTO>(okResult.Value);
            var eq = expectedAlquilerDetailDTO.Equals(alquilerDetailDTOActual);
            Assert.Equal(expectedAlquilerDetailDTO, alquilerDetailDTOActual);
        }
    }
}
