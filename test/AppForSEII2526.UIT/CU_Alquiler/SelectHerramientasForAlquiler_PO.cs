using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.UC_Alquiler
{
    public class SelectHerramientasForAlquiler_PO : PageObject
    {
        By inputNombreHerramienta = By.Id("nombreHerramienta");
        By inputMaterial = By.Id("material");
        By buttonBuscarHerramientas = By.Id("buscarHerramientas");
        By tablaHerramientas = By.Id("tablaHerramientas");
        By errorShownBy = By.Id("ErrorsShown");
        By buttonAlquilarHerramientas = By.Id("alquilarHerramientasButton");
        public SelectHerramientasForAlquiler_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void BuscarHerramientas(string nombreHerramienta, string material)
        {
            WaitForBeingClickable(inputNombreHerramienta);
            WaitForBeingClickable(inputMaterial);   
            _driver.FindElement(inputNombreHerramienta).SendKeys(nombreHerramienta);
            _driver.FindElement(inputMaterial).SendKeys(material);
            _driver.FindElement(buttonBuscarHerramientas).Click();
        }
        public bool CheckListaHerramientas(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, tablaHerramientas);
        }
        public bool CheckMessageError(string errorMessage)
        {
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"Mensaje de error: {actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }
        public void AddHerramientaCarrito(string nombreHerramienta)
        {
            WaitForBeingClickable(By.Id("addHerramienta_" + nombreHerramienta));
            _driver.FindElement(By.Id("addHerramienta_" + nombreHerramienta)).Click();
        }

        public void RemoveMovieFromRentingCart(string nombreHerramienta)
        {
            WaitForBeingClickable(By.Id("removeHerramienta_" + nombreHerramienta));
            _driver.FindElement(By.Id("removeMovie_" + nombreHerramienta)).Click();
        }

        public bool RentingNotAvailable()
        {
            //the button is not Displayed=hidden

            return _driver.FindElement(buttonAlquilarHerramientas).Displayed == false;
        }
    }
}