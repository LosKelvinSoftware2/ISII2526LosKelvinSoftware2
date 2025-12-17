using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.CU_CompraHerramienta
{
    public class CreateCompraPO : PageObject
    {
        // Locators
        private By _inputNombre = By.Id("Name");
        private By _inputApellido = By.Id("Surname");
        private By _inputCorreo = By.Id("Correo");
        private By _inputDireccion = By.Id("DeliveryAddress");
        private By _selectPago = By.Id("PaymentMethod");
        private By _btnComprar = By.Id("Submit");
        private By _btnModifica = By.Id("ModifyItems");

        // Locators de Error
        private By _listaErroresValidacion = By.ClassName("validation-message"); // Errores campo a campo
        private By _resumenErrores = By.ClassName("validation-summary-errors"); // Resumen arriba

        public CreateCompraPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) { }

        // --- ACCIONES ---

        public void RellenarFormulario(string nombre, string apellido, string correo, string direccion, string metodoPago)
        {
            WaitForBeingVisible(_inputNombre);

            _driver.FindElement(_inputNombre).Clear();
            _driver.FindElement(_inputNombre).SendKeys(nombre);

            _driver.FindElement(_inputApellido).Clear();
            _driver.FindElement(_inputApellido).SendKeys(apellido);

            _driver.FindElement(_inputCorreo).Clear();
            _driver.FindElement(_inputCorreo).SendKeys(correo);

            _driver.FindElement(_inputDireccion).Clear();
            _driver.FindElement(_inputDireccion).SendKeys(direccion);

            var selectElement = new SelectElement(_driver.FindElement(_selectPago));
            selectElement.SelectByText(metodoPago);
        }

        public void EstablecerCantidadItem(int indexFila, string cantidad)
        {
            var inputs = _driver.FindElements(By.CssSelector("input[type='number']"));

            if (inputs.Count > indexFila)
            {
                inputs[indexFila].Clear();
                inputs[indexFila].SendKeys(cantidad);
                // A veces Blazor necesita un "Tab" para disparar el evento OnChange
                // inputs[indexFila].SendKeys(Keys.Tab);
            }
        }

        public void EnviarFormulario()
        {
            WaitForBeingClickable(_btnComprar);
            _driver.FindElement(_btnComprar).Click();
        }

        public void ModificarHerramientas()
        {
            WaitForBeingClickable(_btnModifica);
            _driver.FindElement(_btnModifica).Click();
        }

        public void ConfirmarCompraEnModal()
        {
            PressOkModalDialog();
        }

        // --- VERIFICACIONES ---

        public bool HayErroresDeValidacion()
        {
            // Verificamos si existe algún mensaje de error de validación en pantalla
            try
            {
                // Esperamos brevemente por si es asíncrono
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
            var btn = _driver.FindElement(_btnComprar);
            return btn.Enabled;
        }
    }
}