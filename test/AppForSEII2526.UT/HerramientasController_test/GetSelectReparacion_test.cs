using AppForSEII2526.API;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO.RepararDTOs;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UT.HerramientasControllerTest
{
    public class GetSelectReparacion_test : AppForSEII25264SqliteUT
    {
        public GetSelectReparacion_test()
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
                // una herramienta que NO esté en reparación
                new Herramienta(){ Id=5, Nombre="Llave Inglesa", Material="Metal", Precio=40.0f, TiempoReparacion=3, fabricante=fabricante[1]},
            };

            ApplicationUser user = new ApplicationUser()
            {
                Id = "U001",
                Nombre = "Maria",
                Apellido = "Lopez",
                PhoneNumber = "60033344",
                correoelectronico = "marialopez@alu", // <- usa la propiedad real del modelo
                UserName = "marialopez",              // <- típicamente obligatorio en Identity
                NormalizedUserName = "MARIALOPEZ",
                NormalizedEmail = "MARIALOPEZ@ALU",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEHASHASHASHASHAS==", // valor dummy, no importa
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };


            var reparacion = new List<Reparacion>()
            {
                new Reparacion()
                {
                    Id = 1,
                    FechaEntrega = DateTime.Now.AddDays(7),
                    FechaRecogida = DateTime.Now.AddDays(10),
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
                    FechaEntrega = DateTime.Now.AddDays(5),
                    FechaRecogida = DateTime.Now.AddDays(9),
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

        // --- CASOS DE PRUEBA ---
        public static IEnumerable<object[]> TestCasesFor_GetHerramientasDisponiblesParaReparar_OK()
        {
            // recordemos que el método actual devuelve las herramientas EN REPARACIÓN
            var herramientasDTO = new List<HerramientaRepaDTO>()
            {
                new HerramientaRepaDTO(5, "Llave Inglesa", "Metal", "Fabricante2", 40.0f, 3),
            };

            // Sin filtros - solo debería devolver la herramienta disponible
            var tc1 = herramientasDTO.OrderBy(h => h.Id).ToList();

            // Filtrado por nombre que NO existe en herramientas disponibles - lista vacía
            var tc2 = new List<HerramientaRepaDTO>();

            // Filtrado por tiempo de reparación que coincide con la herramienta disponible
            var tc3 = herramientasDTO.Where(h => h.TiempoReparacion == 3).ToList();

            return new List<object[]>
            {
                new object[] { null, null, tc1 },
                new object[] { "NoExiste", null, tc2 },
                new object[] { null, 3, tc3 }
            };
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetHerramientasDisponiblesParaReparar_OK))]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetHerramientasDisponiblesParaReparar_OK_test(string? nombre, int? dias, IList<HerramientaRepaDTO> expected)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HerramientasController>>();
            var controller = new HerramientasController(_context, mockLogger.Object); //mockLogger.Object

            // Act
            var result = await controller.GetHerramientasDisponiblesParaReparar(nombre, dias);

            // Assert
            if (expected.Count == 0)
            {
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
                var value = notFoundResult.Value;
                var mensajeProp = value.GetType().GetProperty("Mensaje");
                Assert.NotNull(mensajeProp);
                var mensaje = mensajeProp.GetValue(value)?.ToString();
                Assert.Equal("No hay herramientas disponibles para reparación.", mensaje);
            }
            else
            {
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var actual = Assert.IsType<List<HerramientaRepaDTO>>(okResult.Value);
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetHerramientasDisponiblesParaReparar_NotFound_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<HerramientasController>>();
            var controller = new HerramientasController(_context, mockLogger.Object);//mockLogger.Object

            // Act -> filtro que no existe
            var result = await controller.GetHerramientasDisponiblesParaReparar("NoExiste", null);

            
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            // El Value es un objeto anónimo con una propiedad "Mensaje"
            var value = notFoundResult.Value;
            // Obtener la propiedad "Mensaje" por reflexión
            var mensajeProp = value.GetType().GetProperty("Mensaje");
            Assert.NotNull(mensajeProp);
            var mensaje = mensajeProp.GetValue(value)?.ToString();
            Assert.Equal("No hay herramientas disponibles para reparación.", mensaje);


        }
    }
}
