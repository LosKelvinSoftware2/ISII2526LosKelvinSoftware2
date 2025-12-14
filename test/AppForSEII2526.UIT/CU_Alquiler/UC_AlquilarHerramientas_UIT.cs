using AppForSEII2526.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Alquiler
{
    public class CU_AlquilerHerramientas_UIT : UC_UIT
    {
        private SelectHerramientasForAlquiler_PO selectHerramientasForAlquiler_PO;
        private CreateAlquiler_PO createAlquiler_PO;
        private DetailsAlquiler_PO detailsAlquiler_PO;

        private const string nombreHerramienta1 = "Clavadora Neumática";
        private const string material1 = "Acero";
        private const string precioHerramienta1 = "140";
        private const string fabricante1 = "Kobalt";

        private const string nombreHerramienta2 = "Cortadora de Azulejos";
        private const string material2 = "Cerámica";
        private const string precioHerramienta2 = "200";
        private const string fabricante2 = "Hitachi";


        public CU_AlquilerHerramientas_UIT(ITestOutputHelper output) : base(output)
        {
            selectHerramientasForAlquiler_PO = new SelectHerramientasForAlquiler_PO(_driver, _output);
            createAlquiler_PO = new CreateAlquiler_PO(_driver, _output);
            detailsAlquiler_PO = new DetailsAlquiler_PO(_driver, _output);
        }
        private void InitialStepsForAlquilarHerramientas()
        {
            //we wait for the option of the menu to be visible
            selectHerramientasForAlquiler_PO.WaitForBeingVisible(By.Id("CreateAlquiler"));
            //we click on the menu
            _driver.FindElement(By.Id("CreateAlquiler")).Click();
        }
        [Fact(Skip = "Este test modifica el estado. Antes de ejecutar lanzar LimpiarAlquileres.sql")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU4_flujoNormal()
        {
            //Arrange
            Initial_step_opening_the_web_page();
            InitialStepsForAlquilarHerramientas();
            selectHerramientasForAlquiler_PO.BuscarHerramientas("", "");
            selectHerramientasForAlquiler_PO.AddHerramientaCarrito("Martillo Neumático");
            selectHerramientasForAlquiler_PO.SeguirConAlquiler();
            
            createAlquiler_PO.RellenarDatos("Juan", "Pérez", "Calle Falsa 123", "Tarjeta de crédito", "600123456", "juanperez@gmail.com", DateTime.Now.AddDays(1), DateTime.Now.AddDays(7));
            createAlquiler_PO.enviarFormulario();
            createAlquiler_PO.confirmarAlquiler();

            var expectedHerramientas = new List<string[]> { new string[] { "Martillo Neumático", "Acero", "150", "1" }, };

            detailsAlquiler_PO.CheckDatos("Juan Pérez" , "Calle Falsa 123" , DateTime.Now.ToString("dd/MM/yyyy") , "900" , DateTime.Now.AddDays(1).ToString("dd/MM/yyyy") , DateTime.Now.AddDays(7).ToString("dd/MM/yyyy"));
            detailsAlquiler_PO.CheckTablaHerramientas(expectedHerramientas);
        }
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU4_AF0_noHerramientasDisponibles()
        {
            //Arrange
            Initial_step_opening_the_web_page();
            InitialStepsForAlquilarHerramientas();
            var expectedMessageError = "No hay herramientas disponibles con los filtros seleccionados.";
            //Act
            selectHerramientasForAlquiler_PO.BuscarHerramientas("herramienta_no_existente", "herramienta_no_existente");
            //Assert
            Assert.True(selectHerramientasForAlquiler_PO.CheckMessageError(expectedMessageError));
        }

        
        [Theory]
        [InlineData(nombreHerramienta1, material1, fabricante1, precioHerramienta1, "Clavadora", "")]
        [InlineData(nombreHerramienta2, material2, fabricante2, precioHerramienta2, "", "Cerámica")]
        [InlineData(nombreHerramienta1, material1, fabricante1, precioHerramienta1, "Clavadora", "Acero")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU4_AF1_filtradoPorNombreYMaterial(string nombreHerramienta, string material, string fabricante, string precioHerramienta,
            string filtroNombre, string filtroMaterial)
        {
            //Arrange
            Initial_step_opening_the_web_page();
            InitialStepsForAlquilarHerramientas();
            var expectedHerramientas = new List<string[]> { new string[] { nombreHerramienta, material, fabricante, precioHerramienta }, };

            //Act
            selectHerramientasForAlquiler_PO.BuscarHerramientas(filtroNombre, filtroMaterial);

            //Assert

            Assert.True(selectHerramientasForAlquiler_PO.CheckListaHerramientas(expectedHerramientas));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU4_AF2_modificarYBorrarHerramientasDelCarrito()
        {
            //Arrange
            Initial_step_opening_the_web_page();
            InitialStepsForAlquilarHerramientas();
            //Act
            selectHerramientasForAlquiler_PO.BuscarHerramientas("", "");

            selectHerramientasForAlquiler_PO.AddHerramientaCarrito("Taladro Percutor");
            selectHerramientasForAlquiler_PO.AddHerramientaCarrito(nombreHerramienta2);
            selectHerramientasForAlquiler_PO.SeguirConAlquiler();
            createAlquiler_PO.ModificarHerramientas();
            selectHerramientasForAlquiler_PO.RemoveAlquilerFromRentingCart("Taladro Percutor");
            selectHerramientasForAlquiler_PO.SeguirConAlquiler();
            var expectedHerramientas = new List<string[]> { new string[] { nombreHerramienta2, material2, precioHerramienta2 }, };
            float expectedPrecio = 1200;
            createAlquiler_PO.RellenarDatos("Juan", "Pérez", "Calle Falsa 123", "Tarjeta de crédito", "600123456", "juanperez@gmail.com", DateTime.Now.AddDays(1), DateTime.Now.AddDays(7));

            //Assert
            //Verificar que la herramienta1 no está en el carrito y la herramienta2 sí
            //Esto se puede hacer comprobando que el botón de alquilar está disponible o no
            Assert.True(createAlquiler_PO.checkPrecioTotal($"{expectedPrecio}"));
            Assert.True(createAlquiler_PO.CheckListaHerramientas(expectedHerramientas));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU4_AF4_CarritoVacioNoPermiteContinuar()
        {
            //Arrange
            Initial_step_opening_the_web_page();
            InitialStepsForAlquilarHerramientas();
            //Act
            selectHerramientasForAlquiler_PO.BuscarHerramientas("", "");
            //Assert
            Assert.True(selectHerramientasForAlquiler_PO.CarritoVacio());
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU4_AF5_DatosFaltantesEnFormulario()
        {
            //Arrange
            Initial_step_opening_the_web_page();
            InitialStepsForAlquilarHerramientas();
            //Act
            selectHerramientasForAlquiler_PO.BuscarHerramientas("", "");
            selectHerramientasForAlquiler_PO.AddHerramientaCarrito("Taladro Percutor");
            selectHerramientasForAlquiler_PO.SeguirConAlquiler();
            createAlquiler_PO.RellenarDatos("", "Pérez", "Calle Falsa 123", "Tarjeta de crédito", "600123456", "PayPal", DateTime.Now.AddDays(1), DateTime.Now.AddDays(7));
            createAlquiler_PO.enviarFormulario();
            var expectedMessageError = "The NombreCliente field is required.";
            //Assert
            Assert.True(createAlquiler_PO.CheckMessageValidation(expectedMessageError));
        }

        public static IEnumerable<object[]> FechasInvalidas =>
        new List<object[]>
        {
            new object[] { "El alquiler debe empezar después de hoy", DateTime.Now.AddDays(-10), DateTime.Now.AddDays(5) },
            new object[] { "El alquiler debe terminar después de iniciar", DateTime.Now.AddDays(10), DateTime.Now.AddDays(5) }
        };
        [Theory]
        [MemberData(nameof(FechasInvalidas))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU4_AF6_fechasNoValidasEnFormulario(string expectedMessageError, DateTime fechaInicio, DateTime fechaFin)
        {
            //Arrange
            Initial_step_opening_the_web_page();
            InitialStepsForAlquilarHerramientas();
            //Act
            selectHerramientasForAlquiler_PO.BuscarHerramientas("", "");
            selectHerramientasForAlquiler_PO.AddHerramientaCarrito("Taladro Percutor");
            selectHerramientasForAlquiler_PO.SeguirConAlquiler();
            createAlquiler_PO.RellenarDatos("Juan", "Pérez", "Calle Falsa 123", "Tarjeta de crédito", "600123456", "PayPal", fechaInicio, fechaFin);
            createAlquiler_PO.enviarFormulario();
            createAlquiler_PO.confirmarAlquiler();
            //Assert
            Assert.True(createAlquiler_PO.CheckMessageError(expectedMessageError));
        }
        }
    }
