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
        private By buttonProcesar = By.Id("Submit"); 
        private By erroresMostrados = By.Id("ErrorsShown"); 

        public PostReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void RellenarDatosCliente(string nombre, string apellido, string username, string telefono, string metodoPago)
        {
            WaitForBeingVisible(inputNombre);

            _driver.FindElement(inputNombre).Clear();
            _driver.FindElement(inputNombre).SendKeys(nombre);

            _driver.FindElement(inputApellido).Clear();
            _driver.FindElement(inputApellido).SendKeys(apellido);

            if (!string.IsNullOrEmpty(username))
            {
                _driver.FindElement(inputUserName).Clear();
                _driver.FindElement(inputUserName).SendKeys(username);
            }

            _driver.FindElement(inputTelefono).Clear();
            _driver.FindElement(inputTelefono).SendKeys(telefono);

            // Seleccionar método de pago del desplegable
            SelectElement selectElement = new SelectElement(_driver.FindElement(selectPago));
            selectElement.SelectByText(metodoPago);
        }

        public void RellenarDetallesItem(string idHerramienta, string cantidad, string descripcion)
        {
            // Localizamos la fila específica de la herramienta 
            By filaLocator = By.Id($"ToolData_{idHerramienta}"); 

            // Descripción tiene un ID específico generado en el razor
            By descLocator = By.Id($"description_{idHerramienta}"); 

            // La cantidad no tiene ID único, está dentro de la fila. La buscamos dentro del contexto de la fila.
            IWebElement fila = _driver.FindElement(filaLocator);
            IWebElement inputCantidad = fila.FindElement(By.CssSelector("input[type='number']")); 

            inputCantidad.Clear();
            inputCantidad.SendKeys(cantidad);

            IWebElement inputDesc = _driver.FindElement(descLocator);
            inputDesc.Clear();
            inputDesc.SendKeys(descripcion);
        }

        public void EnviarReparacion()
        {
            WaitForBeingClickable(buttonProcesar);
            _driver.FindElement(buttonProcesar).Click();
        }

        public void ConfirmarDialogo()
        {
            PressOkModalDialog(); 
        }

        public bool CheckErrorValidacion(string errorEsperado)
        {
            WaitForBeingVisible(erroresMostrados);
            return _driver.FindElement(erroresMostrados).Text.Contains(errorEsperado);
        }
    }
}