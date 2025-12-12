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

        private const string nombreHerramienta1 = "Taladro";
        private const string material1 = "Acero";
        private const string precioHerramienta1 = "70";
        private const string fabricante1 = "Bosch";

        private const string nombreHerramienta2 = "Martillo";
        private const string material2 = "Madera";
        private const string precioHerramienta2 = "30";
        private const string fabricante2 = "Einhell";
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
        [Theory]
        [InlineData(nombreHerramienta1, material1, fabricante1, precioHerramienta1, "Taladro", "")]
        [InlineData(nombreHerramienta2, material2, fabricante2, precioHerramienta2, "", "Madera")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU4_filtering(string nombreHerramienta, string material, string fabricante, string precioHerramienta,
            string filtroNombre, string filtroMaterial)
        {
            Initial_step_opening_the_web_page();
            //Arrange
            InitialStepsForAlquilarHerramientas();
            var expectedHerramientas = new List<string[]> { new string[] { nombreHerramienta, material, fabricante, precioHerramienta }, };

            //Act
            selectHerramientasForAlquiler_PO.BuscarHerramientas(filtroNombre, filtroMaterial);

            //Assert

            Assert.True(selectHerramientasForAlquiler_PO.CheckListaHerramientas(expectedHerramientas));

        }
    }
}
