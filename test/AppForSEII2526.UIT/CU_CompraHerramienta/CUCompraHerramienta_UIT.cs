using AppForSEII2526.UIT.CU_CompraHerramienta;
using AppForSEII2526.UIT.Shared;
using AppForSEII2526.UIT.UC_Alquiler;
using OpenQA.Selenium;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;


namespace AppForSEII2526.UIT.UC_Comprar
{
    public class UC_ComprarHerramientas_UIT : UC_UIT
    {
        private SelectHerramientaCompraPO selectHerramientaPO;
        private CreateCompraPO createCompraPO;
        private DetailsCompraPO detailsCompraPO;

        // --- DATOS DE PRUEBA COMUNES---

        private const string nombreHerramienta1 = "Taladro Percutor";
        private const string materialHerramienta1 = "Acero";
        private const string precioHerramienta1 = "120.50";

        private const string nombreHerramienta2 = "Sierra Circular";
        private const string materialHerramienta2 = "Aluminio";

        private const string usuarioNombre = "Juan";
        private const string usuarioApellido = "Pérez";
        private const string usuarioEmail = "juan.perez@email.com";
        private const string usuarioDireccion = "Calle Mayor 10";

        public UC_ComprarHerramientas_UIT(ITestOutputHelper output) : base(output)
        {
            selectHerramientaPO = new SelectHerramientaCompraPO(_driver, _output);
            createCompraPO = new CreateCompraPO(_driver, _output);
            detailsCompraPO = new DetailsCompraPO(_driver, _output);
        }

        private void InitialStepsForComprarHerramientas()
        {
            //we wait for the option of the menu to be visible
            selectHerramientaPO.WaitForBeingVisible(By.Id("CreateCompra"));
            //we click on the menu
            _driver.FindElement(By.Id("CreateCompra")).Click();
        }

        // --------------------------------------------------
        // CP-01: FLUJO BÁSICO - Compra Exitosa
        // --------------------------------------------------

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_FlujoPrincipalCompleto()
        {
            // Arrange
            Initial_step_opening_the_web_page();
            InitialStepsForComprarHerramientas();

            string metodoPago = "Tarjeta";

            // Buscar y añadir datos 
            selectHerramientaPO.BuscarHerramienta(materialHerramienta1);
            selectHerramientaPO.AnadirAlCarrito(nombreHerramienta1);
            selectHerramientaPO.IrATramitarCompra();

            // Rellenar datos
            createCompraPO.RellenarFormulario(usuarioNombre, usuarioApellido, usuarioEmail, usuarioDireccion, metodoPago);
            createCompraPO.EnviarFormulario();
            createCompraPO.ConfirmarCompraEnModal();

            // Comrpobamos
            Assert.True(detailsCompraPO.VerificarDetallesCompra(usuarioNombre, usuarioApellido, usuarioDireccion, nombreHerramienta1));
        }

        // ---------------------------------------------------------------------------------------
        // CP-02: FLUJO ALTERNATIVO 0 - Sin herramientas disponibles
        // ---------------------------------------------------------------------------------------

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_AF0_NoHerramientasDisponibles()
        {
            // Arrange
            Initial_step_opening_the_web_page();
            InitialStepsForComprarHerramientas();
            var expectedMessageError = "No se han encontrado herramientas";

            // Act - Buscamos un material imposible
            selectHerramientaPO.BuscarHerramienta("Vibranium", "999999");

            // Assert
            bool errorVisible = selectHerramientaPO.CheckMessageError(expectedMessageError);
            bool tablaVacia = _driver.FindElements(By.Id("TableOfHerramientas")).Count == 0;

            Assert.True(errorVisible || tablaVacia);
        }

        // ---------------------------------------------------------------------------------------
        // CP-03, CP-04, CP-05: FLUJO ALTERNATIVO 1 - Filtros
        // ---------------------------------------------------------------------------------------
        [Theory]
        [InlineData(nombreHerramienta2, materialHerramienta2, "", "Aluminio", "")]
        [InlineData(nombreHerramienta1, materialHerramienta1, precioHerramienta1, "Acero", "120,50")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_AF1_Filtrado(string nombreEsperado, string materialEsperado, string precioEsperado,
            string filtroMaterial, string filtroPrecio)
        {

            Initial_step_opening_the_web_page();
            InitialStepsForComprarHerramientas();

            var expectedHerramientas = new List<string[]> {
                new string[] { nombreEsperado, materialEsperado }
            };

            selectHerramientaPO.BuscarHerramienta(filtroMaterial, filtroPrecio);

            Assert.True(selectHerramientaPO.CheckListaHerramientas(expectedHerramientas));
        }

        // ---------------------------------------------------------------------------------------
        // CP-07: FLUJO ALTERNATIVO 3 - Carrito Vacío
        // ---------------------------------------------------------------------------------------

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_AF3_CarritoVacio_BotonDeshabilitado()
        {
            Initial_step_opening_the_web_page();
            InitialStepsForComprarHerramientas();

            Assert.False(selectHerramientaPO.IsButtonComprarEnabled(), "El botón Tramitar no debería estar activo si el carrito está vacío.");
        }

        // ---------------------------------------------------------------------------------------
        // CP-08, CP-13, CP-14: FLUJO ALTERNATIVO 4 - Validaciones del Formulario
        // ---------------------------------------------------------------------------------------
        [Theory]
        [InlineData("", "Pérez", "juan.perez@email.com", "Calle Mayor 10", "Falta Nombre")]
        [InlineData("Juan", "", "juan.perez@email.com", "Calle Mayor 10", "Falta Apellido")]
        [InlineData("Juan", "Pérez", "juan.perez@email.com", "", "Falta Dirección")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_AF4_ValidacionesFormulario(string nombre, string apellido, string correo, string direccion, string casoPrueba)
        {
            // Arrange
            Initial_step_opening_the_web_page();
            InitialStepsForComprarHerramientas();

            // Añadimos Herramienta 1 para llegar al formulario
            selectHerramientaPO.BuscarHerramienta(materialHerramienta1);
            selectHerramientaPO.AnadirAlCarrito(nombreHerramienta1);
            selectHerramientaPO.IrATramitarCompra();

            // Act - Rellenamos con datos del Theory (uno de ellos será inválido)
            createCompraPO.RellenarFormulario(nombre, apellido, correo, direccion, "Tarjeta");
            createCompraPO.EnviarFormulario();

            // Assert
            Assert.True(createCompraPO.HayErroresDeValidacion(), $"Falló la validación para el caso: {casoPrueba}");
        }

        // ---------------------------------------------------------------------------------------
        // CP-11: FLUJO ALTERNATIVO 5 - Cantidad Cero
        // ---------------------------------------------------------------------------------------
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_AF5_ErrorCantidadCero()
        {
            // Arrange
            Initial_step_opening_the_web_page();
            InitialStepsForComprarHerramientas();

            // Añadimos Herramienta 2 (Sierra Circular)
            selectHerramientaPO.BuscarHerramienta(materialHerramienta2);
            selectHerramientaPO.AnadirAlCarrito(nombreHerramienta2);
            selectHerramientaPO.IrATramitarCompra();

            // Act - Datos válidos pero cantidad 0
            createCompraPO.RellenarFormulario(usuarioNombre, usuarioApellido, usuarioEmail, usuarioDireccion, "Efectivo");
            createCompraPO.EstablecerCantidadItem(0, "0");
            createCompraPO.EnviarFormulario();

            // Assert
            Assert.True(createCompraPO.HayErroresDeValidacion(), "Debería mostrar error si la cantidad es 0");
        }

    }



}