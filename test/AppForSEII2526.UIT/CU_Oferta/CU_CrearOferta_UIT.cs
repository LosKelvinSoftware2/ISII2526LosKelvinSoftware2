using AppForSEII2526.UIT.CU_CompraHerramienta;
using AppForSEII2526.UIT.CU_Oferta;

using Microsoft.VisualStudio.TestPlatform.Utilities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UIT.CU_Oferta

{
    public class CU_CrearOferta_UIT : UC_UIT
    {
        private SelectHerramientaForOfertaPO selectHerramientaPO;
        private PostOfertaPO postOfertaPO;
        private DetailOfertaPO detailOfertaPO;

        private const string nombreHerramienta1 = "Taladro Percutor";
        private const string materialHerramienta1 = "Acero";

        private const string fabricante1 = "Makita";

        private const string precioHerramienta1 = "120,5";
        private const string precioHerramientaFinal1 = "30";

        private const string nombreHerramienta2 = "Sierra Circular";
        private const string materialHerramienta2 = "Plástico";
        private const string precioHerramienta2 = "70";

        private const string formatoFecha = "dd/MM/yyyy"; // Definir el formato
        private string fechaInicio = DateTime.Today.AddDays(1).ToString(formatoFecha);
        private string fechaFin = DateTime.Today.AddDays(10).ToString(formatoFecha);
        private string fechaOferta = DateTime.Today.ToString(formatoFecha);

        private string fechaPasado = DateTime.MinValue.ToString();

        private const string metodoPago = "TarjetaCredito";
        private const string dirigidaA = "Socios";

        private const string porcentaje = "50";
        private const string porcentajeNegativo = "-200";

        public CU_CrearOferta_UIT(ITestOutputHelper output) : base(output)
        {
            selectHerramientaPO = new SelectHerramientaForOfertaPO(_driver, _output);
            postOfertaPO = new PostOfertaPO(_driver, _output);
            detailOfertaPO = new DetailOfertaPO(_driver, _output);
        }

        private void InitialStepsForCrearOferta()
        {
            selectHerramientaPO.WaitForBeingVisible(By.Id("Crear Oferta"));
            _driver.FindElement(By.Id("Crear Oferta")).Click();
        }

        //CU_3-FB

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FB()
        {
            //Arrange
            Initial_step_opening_the_web_page();
            InitialStepsForCrearOferta();

            string metodoPago = "TarjetaCredito";
            var expectedHerramientas = new List<string[]> {
                new string[] { nombreHerramienta1, fabricante1, materialHerramienta1, precioHerramienta1}
            };

            selectHerramientaPO.BuscarHerramientas(" ", " ");
            selectHerramientaPO.AddOfertaToRentingCart(nombreHerramienta1);
            selectHerramientaPO.CrearOferta();

            postOfertaPO.RellenarFormulario(fechaInicio, fechaFin, metodoPago, dirigidaA);
            postOfertaPO.EstablecerPorcentaje(0, porcentaje);
            postOfertaPO.EnviarFormulario();
            postOfertaPO.ConfirmarOferta();

            //Assert
            Assert.True(detailOfertaPO.CheckOfertaDetail(dirigidaA, metodoPago, fechaOferta, fechaInicio, fechaFin));
            Assert.True(detailOfertaPO.CheckItem(expectedHerramientas));
        }

        //CU_3-FB-FA0 Filtros
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FB_FA0()
        {
            //Arrange
            Initial_step_opening_the_web_page();
            InitialStepsForCrearOferta();

            var expectedHerramientas = new List<string[]> {
                new string[] { nombreHerramienta1, fabricante1, materialHerramienta1, precioHerramienta1}
            };

            selectHerramientaPO.BuscarHerramientas(fabricante1, precioHerramienta1);

            //Assert
            Assert.True(selectHerramientaPO.CheckListaHerramientas(expectedHerramientas));
        }

        //CU_3-FB-FA1 Periodo incorrecto
        [Theory]
        [InlineData("13/12/2025", "04/02/2026", "TarjetaCredito", "Socios", "50", "Error! La fecha de inicio de oferta debe ser al menos mañana")]
        [InlineData("04/02/2026", "13/12/2025", "TarjetaCredito", "Socios", "50", "Error! La fecha final de oferta debe ser después de la fecha de inicio")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FB_FA1(string fechaInicio, string fechaFin, string metodoPago, string cliente, string porcentaje, string error)
        {
            // Arrange
            Initial_step_opening_the_web_page();
            InitialStepsForCrearOferta();

            // Añadimos Herramienta 1 para llegar al formulario
            selectHerramientaPO.BuscarHerramientas("", "");
            selectHerramientaPO.AddOfertaToRentingCart(nombreHerramienta1);
            selectHerramientaPO.CrearOferta();

            // Act - Rellenamos con datos del Theory (uno de ellos será inválido)
            postOfertaPO.RellenarFormulario(fechaInicio, fechaFin, metodoPago, cliente);
            postOfertaPO.EstablecerPorcentaje(0, porcentaje);
            postOfertaPO.EnviarFormulario();
            postOfertaPO.ConfirmarOferta();

            // Assert
            Assert.True(postOfertaPO.CheckServerErrorText(error), $"Se esperaba el error '{error}' pero no se encontró.");
        }


        //CU_3-FB-FA2 Modificar carrito
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FB_FA2()
        {
            // Arrange
            Initial_step_opening_the_web_page();
            InitialStepsForCrearOferta();

            var expectedHerramientas = new List<string> {
                $"x {nombreHerramienta1} - {precioHerramienta1}€" };

            // Añadimos Herramienta 1 para llegar al formulario
            selectHerramientaPO.BuscarHerramientas("", "");
            selectHerramientaPO.AddOfertaToRentingCart(nombreHerramienta1);
            selectHerramientaPO.AddOfertaToRentingCart(nombreHerramienta2);

            selectHerramientaPO.RemoveOfertaFromRentingCart(nombreHerramienta2);

            // Assert
            Assert.True(selectHerramientaPO.CheckCarrito(expectedHerramientas));
        }

        //CU_3-FB-FA3 Porcentaje incorrecto

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FB_FA3()
        {
            // Arrange
            Initial_step_opening_the_web_page();
            InitialStepsForCrearOferta();

            string metodoPago = "TarjetaCredito";
            var error = "El porcentaje debe estar entre 1 y 100";

            selectHerramientaPO.BuscarHerramientas("", "");
            selectHerramientaPO.AddOfertaToRentingCart(nombreHerramienta1);
            selectHerramientaPO.CrearOferta();

            postOfertaPO.RellenarFormulario(fechaInicio, fechaFin, metodoPago, dirigidaA);
            postOfertaPO.EstablecerPorcentaje(0, porcentajeNegativo);
            postOfertaPO.EnviarFormulario();
            postOfertaPO.ConfirmarOferta();


            // Assert
            Assert.True(postOfertaPO.CheckServerErrorText(error), $"Se esperaba el error '{error}' pero no se encontró.");
        }

        // CU3_FB_FA4 Carrito Vacío
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FB_FA4()
        {
            Initial_step_opening_the_web_page();
            InitialStepsForCrearOferta();

            Assert.True(selectHerramientaPO.OfertarNotAvailable(), "El botón Crear Oferta no debería estar activo si el carrito está vacío.");
        }

        //CU3_FB_FA5 Campos porcentaje sin rellenar
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU3_FB_FA5()
        {
            // Arrange
            Initial_step_opening_the_web_page();
            InitialStepsForCrearOferta();

            string metodoPago = "TarjetaCredito";
            var error = "El porcentaje debe estar entre 1 y 100";

            selectHerramientaPO.BuscarHerramientas("", "");
            selectHerramientaPO.AddOfertaToRentingCart(nombreHerramienta1);
            selectHerramientaPO.CrearOferta();

            postOfertaPO.RellenarFormulario(fechaInicio, fechaFin, metodoPago, dirigidaA);
            postOfertaPO.EstablecerPorcentaje(0, "0");
            postOfertaPO.EnviarFormulario();
            postOfertaPO.ConfirmarOferta();

            // Assert
            Assert.True(postOfertaPO.CheckServerErrorText(error), $"Se esperaba el error '{error}' pero no se encontró.");
        }

    }
}
