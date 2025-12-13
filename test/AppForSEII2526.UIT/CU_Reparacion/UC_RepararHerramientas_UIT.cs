using AppForSEII2526.UIT.Shared;
using Xunit;
using Xunit.Abstractions;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AppForSEII2526.UIT.UC_Reparacion
{
    public class UC_RepararHerramientas_UIT : UC_UIT
    {
        private SelectHerramientasReparacion_PO selectPO;
        private PostReparacion_PO postPO;

        private const string ToolName_Taladro = "Taladro";
        private const string ToolId_Taladro = "1"; // Asumiendo ID 1 para el Taladro
        private const string ToolPrice_Taladro = "10.00";

        public UC_RepararHerramientas_UIT(ITestOutputHelper output) : base(output)
        {
            selectPO = new SelectHerramientasReparacion_PO(_driver, _output);
            postPO = new PostReparacion_PO(_driver, _output);
        }

        private void PasosInicialesParaRepararHerramientas()
        {
            // Navegación a la vista de Selección de Herramientas
            // Nota: Ajusta la URL si la ruta de navegación es diferente
            _driver.Navigate().GoToUrl(_URI + "Reparacion/SelectHerramientaReparacion");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_1_FlujoBasico_Exito()
        {
            
            // Arrange
            Initial_step_opening_the_web_page();
            Perform_login("elena@uclm.es", "Password1234%"); // Precondición: Usuario logueado
            PasosInicialesParaRepararHerramientas();

            // Act - Selección
            selectPO.BuscarHerramientas(ToolName_Taladro, "");
            selectPO.AnadirHerramientaAReparacion(ToolId_Taladro);
            selectPO.ClicTramitarReparacion();

            // Act - Rellenar Formulario (Datos de Esc-1)
            postPO.RellenarDatosCliente("Pepe", "Lopez", "pepeUser", "600123456", "Tarjeta");
            // Detalles del ítem: Cantidad 1, Descripción 'No enciende'
            postPO.RellenarDetallesItem(ToolId_Taladro, "1", "No enciende");

            postPO.EnviarReparacion();
            postPO.ConfirmarDialogo();

            // Assert
            // Verificamos redirección a DetailReparacion
            Assert.Contains("DetailReparacion", _driver.Url);
        }

        [Theory]
        [InlineData("Sierra", "", "Sierra", "Acero", "Bosch", "15.00")]  // UC2_2 Filtrado por Nombre 
        [InlineData("", "5", "Sierra", "Acero", "Bosch", "15.00")]      // UC2_3 Filtrado por Días 
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_2_3_FiltradoHerramientas(string filtroNombre, string filtroDias,
            string expNombre, string expMaterial, string expFabricante, string expPrecio)
        {
            // Escenario 2: Filtrado de herramientas

            // Arrange
            Initial_step_opening_the_web_page();
            PasosInicialesParaRepararHerramientas();

            // Nota: Asumimos ID "2" para Sierra según el contexto del documento de pruebas
            var filasEsperadas = new List<string[]>
            {
                new string[] { "2", expNombre, expMaterial, expPrecio, expFabricante }
            };

            // Act
            selectPO.BuscarHerramientas(filtroNombre, filtroDias);

            // Assert
            Assert.True(selectPO.CheckListaHerramientas(filasEsperadas));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_4_CarritoVacio_BotonInactivo()
        {
            // Escenario 5 (FA3): Intento de continuar con carrito vacío 

            // Arrange
            Initial_step_opening_the_web_page();
            PasosInicialesParaRepararHerramientas();

            // Act
            // No añadimos ninguna herramienta.

            // Assert
            // El botón de tramitar está dentro de un div que se oculta si Items.Count == 0 
            Assert.False(selectPO.EsVisibleBotonTramitar(), "El botón Tramitar no debería ser visible con carrito vacío");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_8_CantidadCero_Error()
        {
            // Escenario 7 (FA5): Validación de cantidad cero

            // Arrange
            Initial_step_opening_the_web_page();
            Perform_login("elena@uclm.es", "Password1234%");
            PasosInicialesParaRepararHerramientas();

            selectPO.BuscarHerramientas(ToolName_Taladro, "");
            selectPO.AnadirHerramientaAReparacion(ToolId_Taladro);
            selectPO.ClicTramitarReparacion();

            // Act
            // Intentamos poner cantidad 0
            postPO.RellenarDetallesItem(ToolId_Taladro, "0", "Rotura");
            postPO.EnviarReparacion();
            postPO.ConfirmarDialogo();

            // Assert
            // No debería redirigir a detalles, debería mantenerse en la página (URL contiene PostReparacion)
            // Y mostrar errores de validación
            Assert.Contains("PostReparacion", _driver.Url);
        }
    }
}