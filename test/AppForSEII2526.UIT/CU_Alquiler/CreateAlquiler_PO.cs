using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.CU_Alquiler
{
    public class CreateAlquiler_PO : PageObject
    {
        By buttonModificarHerramientas = By.Id("ModificarAlquileres");
        By tablaHerramientas = By.Id("TablaAlquilarItems");
        By precioTotal = By.Id("precioTotal");
        By nombreCliente = By.Id("NombreCliente");  
        By apellidoCliente = By.Id("ApellidoCliente");
        By direccionEnvio = By.Id("DireccionEnvio");    
        By metodoPago = By.Id("MetodoPago");
        By numTelefono = By.Id("NumTelefono");
        By correoElectronico = By.Id("CorreoElectronico");
        By fechaInicio = By.Id("FechaInicio");  
        By fechaFin = By.Id("FechaFin");
        By errorShownBy = By.Id("ErrorsShown");
        By validationSummary = By.Id("formularioNoValido");
        By botonconfirmar = By.Id("Button_DialogOK");
        private By ErrorMessageLocator = By.CssSelector("#formularioNoValido li.validation-message");
        public CreateAlquiler_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void ModificarHerramientas()
        {
            WaitForBeingClickable(buttonModificarHerramientas);
            _driver.FindElement(buttonModificarHerramientas).Click();
        }
        public bool CheckListaHerramientas(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, tablaHerramientas);
        }
        
        public void RellenarDatos(string nombre, string apellido, string direccion, string metodoPay, string telefono, string correo, DateTime inicio, DateTime fin)
        { 
            WaitForBeingClickable(nombreCliente);
            _driver.FindElement(nombreCliente).SendKeys(nombre);
            WaitForBeingClickable(apellidoCliente);
            _driver.FindElement(apellidoCliente).SendKeys(apellido);
            WaitForBeingClickable(direccionEnvio);
            _driver.FindElement(direccionEnvio).SendKeys(direccion);
            WaitForBeingClickable(metodoPago);
            new SelectElement(_driver.FindElement(metodoPago)).SelectByText(metodoPay);
            WaitForBeingClickable(numTelefono);
            _driver.FindElement(numTelefono).SendKeys(telefono);
            WaitForBeingClickable(correoElectronico);
            _driver.FindElement(correoElectronico).SendKeys(correo);
            WaitForBeingClickable(fechaInicio);
            InputDateInDatePicker(fechaInicio, inicio);
            WaitForBeingClickable(fechaFin);
            InputDateInDatePicker(fechaFin, fin);
        }
        public void enviarFormulario()
        {
            _driver.FindElement(By.Id("Confirmar")).Click();
        }

        public bool checkPrecioTotal(string expectedPrecioTotal)
        {
            IWebElement actualPrecioTotal = _driver.FindElement(precioTotal);
            _output.WriteLine($"Precio total: {actualPrecioTotal.Text}");
            return actualPrecioTotal.Text.Contains(expectedPrecioTotal);
        }
        public bool CheckMessageValidation(string errorMessage)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementIsVisible(validationSummary));

            IList<IWebElement> errorElements = _driver.FindElements(ErrorMessageLocator);
            return errorElements.Any(e => e.Text.Trim() == errorMessage);
        }
        public bool CheckMessageError(string errorMessage)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementIsVisible(errorShownBy));

            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"Mensaje de error: {actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }
        public void confirmarAlquiler()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            wait.Until(ExpectedConditions.ElementIsVisible(botonconfirmar));
            _driver.FindElement(By.Id("Button_DialogOK")).Click();
        }
    }
}
