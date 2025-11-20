using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTO;
using AppForSEII2526.API.DTO.Comprar_Herramienta;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ComprasController_test
{
    public class GetCompra_test : AppForSEII25264SqliteUT
    {
        // --- Constantes para parámetros ---
        private const string UserId = "U001";
        private const string NombreCliente = "Maria";
        private const string ApellidoCliente = "Lopez";
        private const string CorreoCliente = "marialopez@alu";
        private const string UserName = "marialopez";
        private const string PhoneNumber = "60033344";

        private const string NombreHerramienta1 = "Taladro";
        private const string NombreHerramienta2 = "Martillo";
        private const string NombreHerramienta3 = "Sierra";
        private const string NombreHerramienta4 = "Destornillador";

        public GetCompra_test()
        {
            // Fabricantes
            var fabricante1 = new Fabricante { Id = 1, Nombre = "Fabricante1" };
            var fabricante2 = new Fabricante { Id = 2, Nombre = "Fabricante2" };
            var fabricante3 = new Fabricante { Id = 3, Nombre = "Fabricante3" };
            var fabricantes = new List<Fabricante> { fabricante1, fabricante2, fabricante3 };

            // Herramientas
            var herramienta1 = new Herramienta { Id = 1, Nombre = NombreHerramienta1, Material = "Metal", Precio = 100.0f, TiempoReparacion = 5, fabricante = fabricante1 };
            var herramienta2 = new Herramienta { Id = 2, Nombre = NombreHerramienta2, Material = "Plástico", Precio = 50.0f, TiempoReparacion = 3, fabricante = fabricante2 };
            var herramienta3 = new Herramienta { Id = 3, Nombre = NombreHerramienta3, Material = "Madera", Precio = 75.0f, TiempoReparacion = 4, fabricante = fabricante3 };
            var herramienta4 = new Herramienta { Id = 4, Nombre = NombreHerramienta4, Material = "Metal", Precio = 30.0f, TiempoReparacion = 2, fabricante = fabricante1 };
            var herramientas = new List<Herramienta> { herramienta1, herramienta2, herramienta3, herramienta4 };

            // Usuario
            var user = new ApplicationUser
            {
                Id = UserId,
                Nombre = NombreCliente,
                Apellido = ApellidoCliente,
                PhoneNumber = PhoneNumber,
                correoelectronico = CorreoCliente,
                UserName = UserName,
                NormalizedUserName = UserName.ToUpper(),
                NormalizedEmail = CorreoCliente.ToUpper(),
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEHASHASHASHASHAS==",
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            // Compras
            var compra1 = new Compra
            {
                Id = 1,
                fechaCompra = DateTime.Now,
                PrecioTotal = 200.0f,
                MetodoPago = tiposMetodoPago.Efectivo,
                Cliente = user,
                direccionEnvio = "Avda. España s/n, Albacete 02071", // ← obligatorio
                CompraItems = new List<CompraItem>
                {
                    new CompraItem { compraId = 1, herramientaId = 1, herramienta = herramienta1, cantidad = 1, precio = 10.0f, descripcion = $"Compra de {NombreHerramienta1}" },
                    new CompraItem { compraId = 1, herramientaId = 2, herramienta = herramienta2, cantidad = 2, precio = 15.0f, descripcion = $"Compra de {NombreHerramienta2}" }
                }
            };

            var compra2 = new Compra
            {
                Id = 2,
                fechaCompra = DateTime.Now,
                PrecioTotal = 300.0f,
                MetodoPago = tiposMetodoPago.PayPal,
                Cliente = user,
                direccionEnvio = "Avda. España s/n, Albacete 02071", // ← obligatorio
                CompraItems = new List<CompraItem>
                {
                    new CompraItem { compraId = 2, herramientaId = 3, herramienta = herramienta3, cantidad = 1, precio = 20.0f, descripcion = $"Compra de {NombreHerramienta3}" },
                    new CompraItem { compraId = 2, herramientaId = 4, herramienta = herramienta4, cantidad = 3, precio = 25.0f, descripcion = $"Compra de {NombreHerramienta4}" }
                }
            };

            _context.Fabricante.AddRange(fabricantes);
            _context.Herramienta.AddRange(herramientas);
            _context.ApplicationUser.Add(user);
            _context.Compra.AddRange(new List<Compra> { compra1, compra2 });
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCompra_NotFound_test()
        {
            var mock = new Mock<ILogger<CompraController>>();
            var logger = mock.Object;
            var controller = new CompraController(_context, logger);

            var result = await controller.GetCompraDetails(0);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetCompra_OK_test()
        {
            var mock = new Mock<ILogger<CompraController>>();
            var logger = mock.Object;
            var controller = new CompraController(_context, logger);

            var result = await controller.GetCompraDetails(1);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<CompraDetailsDTO>(okResult.Value);

            Assert.Equal(NombreCliente, dto.nombreCliente);
            Assert.Equal(ApellidoCliente, dto.apellidoCliente);
            Assert.Equal(CorreoCliente, dto.correoCliente);
            Assert.Equal(200.0f, dto.PrecioTotal);
            Assert.Equal(tiposMetodoPago.Efectivo, dto.MetodoPago);

            Assert.Equal(2, dto.CompraItems.Count);
            Assert.Equal(NombreHerramienta1, dto.CompraItems[0].nombre);
            Assert.Equal(1, dto.CompraItems[0].cantidad);
            Assert.Equal(10.0f, dto.CompraItems[0].precio);
            Assert.Equal($"Compra de {NombreHerramienta1}", dto.descripcion);

            Assert.Equal(NombreHerramienta2, dto.CompraItems[1].nombre);
            Assert.Equal(2, dto.CompraItems[1].cantidad);
            Assert.Equal(15.0f, dto.CompraItems[1].precio);
            Assert.Equal($"Compra de {NombreHerramienta1}", dto.descripcion);
        }
    }
}
