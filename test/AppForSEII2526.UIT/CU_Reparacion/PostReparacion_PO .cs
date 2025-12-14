using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Reparacion
{
    public class PostReparacion_PO : PageObject
    {
        // Localizadores basados en PostReparacion.razor
        private By inputNombre = By.Id("Name"); 
        private By inputApellido = By.Id("Surname"); 
        private By inputUserName = By.Id("Username"); 
        private By inputTelefono = By.Id("NumTelefono"); 
        private By selectPago = By.Id("PaymentMethod");
        private By buttonSubmit = By.Id("Submit");
        
        private By errorsShown = By.Id("ErrorsShown"); 
        private By validationSummary = By.ClassName("validation-summary-errors");

        public PostReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void RellenarFormulario(string nombre, string apellido, string username, string telefono, string metodoPago)
        {
            WaitForBeingVisible(inputNombre);

            _driver.FindElement(inputNombre).Clear();
            _driver.FindElement(inputNombre).SendKeys(nombre);

            _driver.FindElement(inputApellido).Clear();
            _driver.FindElement(inputApellido).SendKeys(apellido);

            _driver.FindElement(inputUserName).Clear();
            _driver.FindElement(inputUserName).SendKeys(username);

            _driver.FindElement(inputTelefono).Clear();
            _driver.FindElement(inputTelefono).SendKeys(telefono);

            var selectElement = new SelectElement(_driver.FindElement(selectPago));
            selectElement.SelectByText(metodoPago);
        }

        //public void SetItemDetails(string toolId, string quantity, string description)
        //{
            
        //    By rowLocator = By.Id($"ToolData_{toolId}");
            
        //    By descLocator = By.Id($"description_{toolId}");
            
        //    IWebElement row = _driver.FindElement(rowLocator);
        //    IWebElement qtyInput = row.FindElement(By.CssSelector("input[type='number']"));

        //    qtyInput.Clear();
        //    qtyInput.SendKeys(quantity);

        //    IWebElement descInput = _driver.FindElement(descLocator);
        //    descInput.Clear();
        //    descInput.SendKeys(description);
        //}

        public void EstablecerCantidadItem(int indexFila, string cantidad)
        {
            var inputs = _driver.FindElements(By.CssSelector("input[type='number']"));

            if (inputs.Count > indexFila)
            {
                inputs[indexFila].Clear();
                inputs[indexFila].SendKeys(cantidad);
            }
        }

        public void SubmitReparacion()
        {
            WaitForBeingClickable(buttonSubmit);
            _driver.FindElement(buttonSubmit).Click();
        }

        public void ConfirmDialog()
        {
            // El dialogo genérico definido en PageObject (Dialog.razor)
            PressOkModalDialog();
        }

        public bool HayErroresDeValidacion(string error)
        {
            // Verifica tanto el resumen de validación como el mensaje de error general
            try
            {
                // Esperamos brevemente por si es asíncrono
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
                return wait.Until(d =>
                    d.FindElements(errorsShown).Count > 0 ||
                    d.FindElements(validationSummary).Count > 0
                );
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
        public bool IsBotonGuardarActivo()
        {
            var btn = _driver.FindElement(buttonSubmit);
            return btn.Enabled;
        }
    }
}