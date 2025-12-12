using AppForSEII2526.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Alquiler
{
    public class UC_AlquilerHerramientas_UIT : UC_UIT
    {
        private SelectHerramientasForAlquiler_PO selectHerramientasForAlquiler_PO;

        private const string nombreHerramienta1 = "Clavadora Neumática";
        private const string material1 = "Acero";
        private const string precioHerramienta1 = "140";
        private const string fabricante1 = "Kobalt";

        private const string nombreHerramienta2 = "Cortadora de Azulejos";
        private const string material2 = "Cerámica";
        private const string precioHerramienta2 = "200";
        private const string fabricante2 = "Hitachi";
        public UC_AlquilerHerramientas_UIT(ITestOutputHelper output) : base(output)
        {
            selectHerramientasForAlquiler_PO = new SelectHerramientasForAlquiler_PO(_driver, _output);
        }
        private void InitialStepsForAlquilarHerramientas()
        {
            //we wait for the option of the menu to be visible
            selectHerramientasForAlquiler_PO.WaitForBeingVisible(By.Id("CreateAlquiler"));
            //we click on the menu
            _driver.FindElement(By.Id("CreateAlquiler")).Click();
        }
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU4_flujoPrincipalCompleto (){
            //Arrange
            Initial_step_opening_the_web_page();
            InitialStepsForAlquilarHerramientas();


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
            selectHerramientasForAlquiler_PO.BuscarHerramientas("herramienta_no_existente","herramienta_no_existente");
            //Assert
            Assert.True(selectHerramientasForAlquiler_PO.CheckMessageError(expectedMessageError));
        }
        [Theory]
        [InlineData(nombreHerramienta1, material1, fabricante1, precioHerramienta1, "Clavadora", "")]
        [InlineData(nombreHerramienta2, material2, fabricante2, precioHerramienta2, "", "Cerámica")]
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
    }
}
