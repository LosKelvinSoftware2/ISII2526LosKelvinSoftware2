using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace AppForSEII2526.UIT.CU_Oferta
{
    internal class SelectHerramientaForOfertaPO: PageObject
    {
        By inputFabricante = By.Id("inputFabricante");
        By inputPrecio = By.Id("inputPrecio");
        By buttonBuscarHerramientas = By.Id("BuscarHerramientas");
        By tablaHerramientas = By.Id("TableOfHerramientas");
        By tablaCarrito = By.Id("Carrito");
        By errorShownBy = By.Id("ErrorsShown");
        By buttonCrearOferta = By.Id("ofertarHerramientaButton");
        By _cartItemButtons = By.CssSelector(".col-2 button[id^='removeHerramienta_']");


        public SelectHerramientaForOfertaPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) { }

        public void BuscarHerramientas(string? fabricante, string? precio)
        {
            WaitForBeingClickable(inputFabricante);
            WaitForBeingClickable(inputPrecio);
            _driver.FindElement(inputFabricante).SendKeys(fabricante);
            _driver.FindElement(inputPrecio).SendKeys(precio);
            _driver.FindElement(buttonBuscarHerramientas).Click();
        }
        public bool CheckListaHerramientas(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, tablaHerramientas);
        }

        public bool CheckCarrito(List<string> expectedItemTexts)
        {
            // 1. Encontrar todos los botones que representan ítems en el carrito
            var actualItems = _driver.FindElements(_cartItemButtons);

            // 2. Verificar el número de ítems
            if (actualItems.Count != expectedItemTexts.Count)
            {
                _output.WriteLine($"Error en CheckCarrito: Esperaba {expectedItemTexts.Count} items, encontré {actualItems.Count}.");
                return false;
            }

            // 3. Verificar el texto de cada ítem
            for (int i = 0; i < expectedItemTexts.Count; i++)
            {
                string actualText = actualItems[i].Text.Trim();
                string expectedText = expectedItemTexts[i].Trim();

                if (actualText != expectedText)
                {
                    _output.WriteLine($"Error en CheckCarrito: Item {i} - Esperaba texto: '{expectedText}', encontré: '{actualText}'.");
                    return false;
                }
            }

            return true;
        }

        public bool CheckMessageError(string errorMessage)
        {
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }

        public void AddOfertaToRentingCart(string nombreHerramienta)
        {
            WaitForBeingClickable(By.Id("herramientaToOferta_" + nombreHerramienta));
            _driver.FindElement(By.Id("herramientaToOferta_" + nombreHerramienta)).Click();
        }

        public void CrearOferta()
        {
            WaitForBeingVisible(buttonCrearOferta);
            _driver.FindElement(buttonCrearOferta).Click();
        }

        public void RemoveOfertaFromRentingCart(string nombreHerramienta)
        {
            WaitForBeingClickable(By.Id("removeHerramienta_" + nombreHerramienta));
            _driver.FindElement(By.Id("removeHerramienta_" + nombreHerramienta)).Click();
        }

        public bool OfertarNotAvailable()
        {
            //the button is not Displayed=hidden

            return _driver.FindElement(buttonCrearOferta).Displayed == false;
        }
    }
}
