using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.CU_CompraHerramienta
{
    public class SelectHerramientaCompraPO : PageObject
    {
        private By _inputName = By.Id("inputMaterial");
        private By _inputPrecio = By.Id("inputPrecio");
        private By _btnBuscar = By.Id("buscarHerramientas");
        private By _btnTramitar = By.Id("TramitarCompra");
        private By _tablaHerramientas = By.Id("TableOfHerramientas");

        public SelectHerramientaCompraPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) { }

        public void BuscarHerramienta(string nombre, string precio = "")
        {
            WaitForBeingVisible(_inputName);

            _driver.FindElement(_inputName).Clear();
            _driver.FindElement(_inputName).SendKeys(nombre);

            if (!string.IsNullOrEmpty(precio))
            {
                _driver.FindElement(_inputPrecio).Clear();
                _driver.FindElement(_inputPrecio).SendKeys(precio);
            }

            _driver.FindElement(_btnBuscar).Click();

            // Esperamos a que la tabla aparezca para confirmar que la búsqueda terminó
            WaitForBeingVisible(_tablaHerramientas);
        }

        public void AnadirAlCarrito(string nombreHerramienta)
        {
            // El ID es dinámico, es decir, puede cambiar: id="herramientaParaComprar_@h.Nombre"
            By btnAnadir = By.Id($"herramientaParaComprar_{nombreHerramienta}");

            WaitForBeingVisible(btnAnadir);
            WaitForBeingClickable(btnAnadir);
            _driver.FindElement(btnAnadir).Click();
        }

        public void IrATramitarCompra()
        {
            WaitForBeingVisible(_btnTramitar);
            _driver.FindElement(_btnTramitar).Click();
        }
    }
}