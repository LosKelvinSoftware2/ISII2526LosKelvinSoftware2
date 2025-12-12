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
        // IDs extraídos de CreateCompra.razor
        private By _inputNombre = By.Id("Name");
        private By _inputApellido = By.Id("Surname");
        private By _inputCorreo = By.Id("Correo");
        private By _inputDireccion = By.Id("DeliveryAddress");
        private By _selectPago = By.Id("PaymentMethod");
        private By _btnComprar = By.Id("Submit");

        public CreateCompraPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) { }

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

            // Seleccionar en el desplegable
            var selectElement = new SelectElement(_driver.FindElement(_selectPago));
            selectElement.SelectByText(metodoPago);
        }

        public void EnviarFormulario()
        {
            WaitForBeingClickable(_btnComprar);
            _driver.FindElement(_btnComprar).Click();
        }

        public void ConfirmarCompraEnModal()
        {
            // Usamos el método de PageObject para confirmar el diálogo
            PressOkModalDialog();
        }
    }
}