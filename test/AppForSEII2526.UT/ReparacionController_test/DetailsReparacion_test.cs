using AppForSEII2526.API;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.RepararDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ReparacionController_test
{
    public class DetailsReparacion_test : AppForSEII25264SqliteUT
    {
         // Definir fechas fijas para evitar problemas de tiempo
        private readonly DateTime fechaBase = new DateTime(2025, 11, 21, 18, 59, 25);

        public DetailsReparacion_test()
        {
            // --- CREAMOS LOS DATOS DE PRUEBA ---
            var fabricante = new List<Fabricante>()
            {
                new Fabricante(){ Id=1, Nombre="Fabricante1"},
                new Fabricante(){ Id=2, Nombre="Fabricante2"},
                new Fabricante(){ Id=3, Nombre="Fabricante3"}
            };

            var herramienta = new List<Herramienta>()
            {
                new Herramienta(){ Id=1, Nombre="Taladro", Material="Metal", Precio=100.0f, TiempoReparacion=5, fabricante=fabricante[0]},
                new Herramienta(){ Id=2, Nombre="Martillo", Material="Plástico", Precio=50.0f, TiempoReparacion=3, fabricante=fabricante[1]},
                new Herramienta(){ Id=3, Nombre="Sierra", Material="Madera", Precio=75.0f, TiempoReparacion=4, fabricante=fabricante[2]},
                new Herramienta(){ Id=4, Nombre="Destornillador", Material="Metal", Precio=30.0f, TiempoReparacion=2, fabricante=fabricante[0]},
            };

            ApplicationUser user = new ApplicationUser()
            {
                Id = "U001",
                Nombre = "Maria",
                Apellido = "Lopez",
                PhoneNumber = "60033344",
                correoelectronico = "marialopez@alu",
                UserName = "marialopez",
                NormalizedUserName = "MARIALOPEZ",
                NormalizedEmail = "MARIALOPEZ@ALU",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEHASHASHASHASHAS==",
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            var reparacion = new List<Reparacion>()
            {
                new Reparacion()
                {
                    Id = 1,
                    FechaEntrega = fechaBase.AddDays(7),
                    FechaRecogida = fechaBase.AddDays(10),
                    PrecioTotal = 200.0f,
                    MetodoPago = tiposMetodoPago.Efectivo,
                    Cliente = user,
                    ItemsReparacion = new List<ReparacionItem>()
                    {
                        new ReparacionItem() { reparacionId = 1, herramientaId = 1, Herramienta = herramienta[0], Cantidad = 1, Precio = 10.0f, Descripcion = "Reparación de Taladro" },
                        new ReparacionItem() { reparacionId = 1, herramientaId = 2, Herramienta = herramienta[1], Cantidad = 2, Precio = 15.0f, Descripcion = "Reparación de Martillo" }
                    }
                },
                new Reparacion()
                {
                    Id = 2,
                    FechaEntrega = fechaBase.AddDays(5),
                    FechaRecogida = fechaBase.AddDays(9),
                    PrecioTotal = 300.0f,
                    MetodoPago = tiposMetodoPago.PayPal,
                    Cliente = user,
                    ItemsReparacion = new List<ReparacionItem>()
                    {
                        new ReparacionItem() { reparacionId = 2, herramientaId = 3, Herramienta = herramienta[2], Cantidad = 1, Precio = 20.0f , Descripcion = "Reparación de Sierra" },
                        new ReparacionItem() { reparacionId = 2, herramientaId = 4, Herramienta = herramienta[3], Cantidad = 3, Precio = 25.0f , Descripcion = "Reparación de Destornillador"}
                    }
                }
            };

            _context.Fabricante.AddRange(fabricante);
            _context.Herramienta.AddRange(herramienta);
            _context.ApplicationUser.Add(user);
            _context.Reparacion.AddRange(reparacion);
            _context.SaveChanges();
        }

        // ---------------------------------------------------------
        //               TEST 1: NOT FOUND
        // ---------------------------------------------------------

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReparacionDetails_NotFound_test()
        {
            // Arrange            
            var mockLogger = new Mock<ILogger<ReparacionController>>();
            var controller = new ReparacionController(_context, mockLogger.Object);

            // Eliminar todas las reparaciones para este test
            //_context.Reparacion.RemoveRange(_context.Reparacion);
            //await _context.SaveChangesAsync();

            // Act
            var result = await controller.GetReparacionDetails(0);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualList = Assert.IsType<List<ReparacionDetailsDTO>>(okResult.Value);
            Assert.Empty(actualList);


            //Assert.IsType<NotFoundObjectResult>(result);
            //Assert.Equal("No se encontraron reparaciones registradas.", result.ToString());
        }

        // ---------------------------------------------------------
        //               TEST 2: FOUND
        // ---------------------------------------------------------

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReparacionDetails_Found_test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ReparacionController>>();
            var controller = new ReparacionController(_context, mockLogger.Object);

            // Act
            var result = await controller.GetReparacionDetails(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualList = Assert.IsType<List<ReparacionDetailsDTO>>(okResult.Value);
            Assert.Single(actualList); // Debe haber solo 1
            var actualReparacion = actualList[0];

            // el objeto esperado
            var expectedReparacion = new ReparacionDetailsDTO(
                1,
                "Maria",
                "Lopez",
                "60033344",
                fechaBase.AddDays(7),
                fechaBase.AddDays(10),
                200.0f,
                tiposMetodoPago.Efectivo,
                new List<ReparacionItemDTO>()
                {
                    new ReparacionItemDTO("Taladro", 10.0f, 1, "Reparación de Taladro"),
                    new ReparacionItemDTO("Martillo", 15.0f, 2, "Reparación de Martillo")
                }
            );
            // El UserName es nulo
            //expectedReparacion.UserName = null;
            Assert.Equal(expectedReparacion, actualReparacion);
        }



    }
}
