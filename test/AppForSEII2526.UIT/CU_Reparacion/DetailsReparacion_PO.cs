using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UIT.UC_Reparacion
{
    public class DetailsReparacion_PO : PageObject
    {
        // Localizadores basados en DetailsReparacion.razor
        private By labelNameSurname = By.Id("NameSurname"); 
        private By labelPaymentMethod = By.Id("PaymentMethod");
        private By labelNumTelefono = By.Id("NumTelefono");
        private By labelFechaEntrega = By.Id("FechaEntrega"); 
        private By labelFechaRecogida = By.Id("FechaRecogida"); 
        private By tablaHerramientas = By.Id("Herramientas"); 
        private By labelTotalPrice = By.Id("TotalPrice"); 

        public DetailsReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool VerificarDetallesReparacion(string nombreCompleto, string metodoPago, string precioTotal)
        {
            
            try
            {
                // Esperamos que todos los elementos sean visibles
                WaitForBeingVisible(labelNameSurname);
                WaitForBeingVisible(labelNumTelefono);
                WaitForBeingVisible(labelTotalPrice);
                WaitForBeingVisible(labelPaymentMethod);
                WaitForBeingVisible(tablaHerramientas);


                string actualName = _driver.FindElement(labelNameSurname).Text;
                string actualPayment = _driver.FindElement(labelPaymentMethod).Text;
                string actualNumTelefono = _driver.FindElement(labelNumTelefono).Text;
                string actualPrice = _driver.FindElement(labelTotalPrice).Text;

                // Debug
                _output.WriteLine($"Actual Name: {actualName} vs Expected: {nombreCompleto}");
                _output.WriteLine($"Actual Price: {actualPrice} vs Expected: {precioTotal}");

                bool checkName = actualName.Contains(nombreCompleto);
                bool checkNumTelefono = !string.IsNullOrEmpty(actualNumTelefono);
                bool checkPayment = actualPayment.Contains(metodoPago);
                bool checkPrice = actualPrice.Contains(precioTotal);

                return checkName && checkPayment && checkPrice;
            }
            catch (Exception ex)
            {
                _output.WriteLine("Excepción verificando detalles: " + ex.Message);
                return false;
            }
        }

        public string ObtenerFechaRecogidaTexto()
        {
            WaitForBeingVisible(labelFechaRecogida);
            string textoCompleto = _driver.FindElement(labelFechaRecogida).Text;

            // Tu Razor muestra: "dd/MM/yyyy HH:mm:ss" (ej: 18/12/2025 00:00:00)
            // Nosotros solo queremos la fecha (los primeros 10 caracteres)
            if (textoCompleto.Length >= 10)
            {
                return textoCompleto.Substring(0, 10);
            }
            return textoCompleto;
        }

        public bool CheckListaHerramientasReparadas(List<string[]> expectedRows)
        {
            
            return CheckBodyTable(expectedRows, tablaHerramientas);
        }
        public bool CheckPrecioTotal(string precioEsperado)
        {
            WaitForBeingVisible(labelTotalPrice);
            string textoPrecio = _driver.FindElement(labelTotalPrice).Text;
            return textoPrecio.Contains(precioEsperado);
        }
    }
}