using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Reparacion
{
    public class DetailsReparacion_PO : PageObject
    {
        // Localizadores extraídos de DetailsReparacion.razor
        private By labelNameSurname = By.Id("NameSurname");
        private By labelPaymentMethod = By.Id("PaymentMethod");
        private By labelFechaEntrega = By.Id("FechaEntrega");
        private By labelFechaRecogida = By.Id("FechaRecogida");
        private By tablaHerramientas = By.Id("Herramientas");
        private By labelTotalPrice = By.Id("TotalPrice");

        public DetailsReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        // Método para validar los datos generales de la reparación
        public bool CheckDetallesReparacion(string nombreCompleto, string metodoPago, string precioTotal)
        {
            WaitForBeingVisible(labelNameSurname);

            string actualName = _driver.FindElement(labelNameSurname).Text;
            string actualPayment = _driver.FindElement(labelPaymentMethod).Text;
            string actualPrice = _driver.FindElement(labelTotalPrice).Text;

            bool checkName = actualName.Contains(nombreCompleto);
            bool checkPayment = actualPayment.Contains(metodoPago);
            bool checkPrice = actualPrice.Contains(precioTotal);

            // Logging para depuración en caso de fallo
            if (!checkName) _output.WriteLine($"Error Nombre: Esperado '{nombreCompleto}', Actual '{actualName}'");
            if (!checkPayment) _output.WriteLine($"Error Pago: Esperado '{metodoPago}', Actual '{actualPayment}'");
            if (!checkPrice) _output.WriteLine($"Error Precio: Esperado '{precioTotal}', Actual '{actualPrice}'");

            return checkName && checkPayment && checkPrice;
        }

        // Método para verificar fechas si es necesario (Opcional según tus casos de prueba)
        public bool CheckFechas(string fechaEntregaEsperada)
        {
            WaitForBeingVisible(labelFechaEntrega);
            return _driver.FindElement(labelFechaEntrega).Text.Contains(fechaEntregaEsperada);
        }

        // Método para validar la tabla de herramientas reparadas
        public bool CheckListaHerramientasReparadas(List<string[]> herramientasEsperadas)
        {
            // Mapea a la tabla con id="Herramientas" en el Razor
            // Estructura de columnas esperada en CheckBodyTable: 
            // 1. Herramienta, 2. Descripción, 3. Cantidad, 4. Precio 
            return CheckBodyTable(herramientasEsperadas, tablaHerramientas);
        }
    }
}