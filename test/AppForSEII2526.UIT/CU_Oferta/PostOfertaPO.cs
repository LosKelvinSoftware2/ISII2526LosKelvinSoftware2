using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UIT.CU_Oferta
{
    internal class PostOfertaPO : PageObject
    {
        //Locators
        By inputfechaInicio = By.Id("fechaInicio");
        By inputfechaFin = By.Id("fechaFinal");
        By inputMetodoPago = By.Id("PaymentMethod");
        By inputDirigidaA = By.Id("Customers");
        private By btnOfertar= By.Id("Submit");

        //Locators de error
        private By _listaErroresValidacion = By.ClassName("validation-message"); // Errores campo a campo
        private By _resumenErrores = By.ClassName("validation-summary-errors"); // Resumen arriba

        // Apunta al mensaje de error específico del servidor (el "log error")
        private By _serverErrorDisplay = By.Id("ErrorsShown");

        public PostOfertaPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) { }
    

        public void RellenarFormulario(string fechaInicio, string fechaFin, string metodoPago, string dirigidaA)
        {
            WaitForBeingVisible(inputfechaInicio);

            _driver.FindElement(inputfechaInicio).Clear();
            _driver.FindElement(inputfechaInicio).SendKeys(fechaInicio);

            _driver.FindElement(inputfechaFin).Clear();
            _driver.FindElement(inputfechaFin).SendKeys(fechaFin);

            var selectPayMethod = new SelectElement(_driver.FindElement(inputMetodoPago));
            selectPayMethod.SelectByText(metodoPago);

            var selectCustomers = new SelectElement(_driver.FindElement(inputDirigidaA));
            selectCustomers.SelectByText(dirigidaA);
        }

        public void EstablecerPorcentaje(int indexFila, string porcentaje)
        {
            var inputs = _driver.FindElements(By.CssSelector("input[type='number']"));

            if (inputs.Count > indexFila)
            {
                inputs[indexFila].Clear();
                inputs[indexFila].SendKeys(porcentaje);
            }
        }

        public void EnviarFormulario()
        {
            WaitForBeingClickable(btnOfertar);
            _driver.FindElement(btnOfertar).Click();
        }

        public void ConfirmarOferta()
        {
            PressOkModalDialog();
        }

        // --- VERIFICACIONES ---

        public bool CheckServerErrorText(string expectedErrorMessage)
        {
            try
            {
                // Espera a que el elemento que contiene el error del servidor sea visible
                WaitForBeingVisible(_serverErrorDisplay);

                // Obtiene el texto completo del error
                string actualErrorText = _driver.FindElement(_serverErrorDisplay).Text;

                // **Loguea el error real encontrado** (Respuesta a tu solicitud)
                _output.WriteLine($"Error actual encontrado: {actualErrorText}");

                // Compara si el texto contiene el mensaje esperado
                return actualErrorText.Contains(expectedErrorMessage);
            }
            catch (WebDriverTimeoutException)
            {
                // Si el elemento nunca aparece, retorna false
                _output.WriteLine("El elemento de error ('ErrorsShown') no apareció a tiempo.");
                return false;
            }
        }

        public bool HayValidationErrors()
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
                return wait.Until(d =>
                    d.FindElements(_listaErroresValidacion).Count > 0 ||
                    d.FindElements(_resumenErrores).Count > 0
                );
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public bool IsBotonGuardarActivo()
        {
            var btn = _driver.FindElement(btnOfertar);
            return btn.Enabled;
        }

    }
}
