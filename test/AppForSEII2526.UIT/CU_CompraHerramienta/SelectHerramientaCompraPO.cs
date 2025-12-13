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
        // Locators
        private By _inputMaterial = By.Id("inputMaterial");
        private By _inputPrecio = By.Id("inputPrecio");
        private By _btnBuscar = By.Id("buscarHerramientas");
        private By _btnTramitar = By.Id("TramitarCompra");
        private By _tablaHerramientas = By.Id("TableOfHerramientas");
        private By _mensajeError = By.Id("ErrorsShown");

        public SelectHerramientaCompraPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) { }

        // --- ACCIONES ---

        public void BuscarHerramienta(string material, string precio = "")
        {
            WaitForBeingVisible(_inputMaterial);
            _driver.FindElement(_inputMaterial).Clear();
            _driver.FindElement(_inputMaterial).SendKeys(material);

            if (!string.IsNullOrEmpty(precio))
            {
                _driver.FindElement(_inputPrecio).Clear();
                _driver.FindElement(_inputPrecio).SendKeys(precio);
            }

            _driver.FindElement(_btnBuscar).Click();

            // Esperamos un momento a que la tabla se refresque o aparezca
            try { 
                WaitForBeingVisible(_tablaHerramientas); 
            } 
            catch { 
                /* Puede no aparecer si no hay resultados */ 
            }


        }

        public void AnadirAlCarrito(string nombreHerramienta)
        {
            // ID dinámico del botón "Añadir"
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

        // --- VERIFICACIONES (ASSERTS) ---

        public bool CheckMessageError(string mensajeEsperado)
        {
            try
            {
                WaitForBeingVisible(_mensajeError);
                string textoActual = _driver.FindElement(_mensajeError).Text;
                return textoActual.Contains(mensajeEsperado);
            }
            catch (WebDriverTimeoutException)
            {
                return false; // No apareció el mensaje
            }
        }

        public bool CheckListaHerramientas(List<string[]> herramientasEsperadas)
        {
            try
            {
                WaitForBeingVisible(_tablaHerramientas);
                string textoTabla = _driver.FindElement(_tablaHerramientas).Text;

                foreach (var h in herramientasEsperadas)
                {
                    // h[0] = Nombre, h[1] = Material, h[2] = Precio (según lo que envíes en el test)
                    if (!textoTabla.Contains(h[0]) || !textoTabla.Contains(h[1]))
                    {
                        _output.WriteLine($"Falta en tabla: {h[0]} o {h[1]}");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsButtonComprarEnabled()
        {
            try
            {
                // Si el botón está oculto o deshabilitado, esto saltará o devolverá false
                var btn = _driver.FindElement(_btnTramitar);
                return btn.Displayed && btn.Enabled;
            }
            catch (NoSuchElementException)
            {
                return false; // El botón ni siquiera existe en el DOM (carrito vacío)
            }
        }
    }
}