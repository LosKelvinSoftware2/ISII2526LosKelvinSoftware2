using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.UIT.CU_Oferta
{
    internal class DetailOfertaPO: PageObject
    {
        By labelDirigida = By.Id("Customers");
        By labelMetPago = By.Id("PaymentMethod");
        By labelfechaOferta = By.Id("OfertaDate");
        By labelperiodoOferta = By.Id("OfertaPeriod");
        By tablaHerramientas = By.Id("OfferedTools");

        public DetailOfertaPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) { }

        public bool CheckOfertaDetail(string cliente, string metodoPago, string fechaOferta, string fechaInicio, string fechaFin)
        {
            try
            {
                WaitForBeingVisible(labelDirigida);
                WaitForBeingVisible(labelMetPago);
                WaitForBeingVisible(labelfechaOferta);
                WaitForBeingVisible(labelperiodoOferta);

                string tipoCliente = _driver.FindElement(labelDirigida).Text;
                string metodoDePago = _driver.FindElement(labelMetPago).Text;
                string fechaDeOferta = _driver.FindElement(labelfechaOferta).Text;
                string periodoDeOferta = _driver.FindElement(labelperiodoOferta).Text;

                // Validaciones
                bool clienteOk = tipoCliente.Contains(cliente);
                bool metodoPagoOk = metodoDePago.Contains(metodoPago);
                bool fechaOfertaOk = fechaDeOferta.Contains(fechaOferta);
                bool periodoOfertaOk = periodoDeOferta.Contains(fechaInicio)
                    && periodoDeOferta.Contains(fechaFin);

                // Logs de error para saber qué falló
                if (!clienteOk) _output.WriteLine($"Error Cliente: Esperaba '{cliente}', veo '{tipoCliente}'");
                if (!metodoPagoOk) _output.WriteLine($"Error Método de Pago: Esperaba '{metodoPago}', veo '{metodoDePago}'");
                if (!fechaOfertaOk) _output.WriteLine($"Error Fecha de Oferta: Esperaba '{fechaOferta}', veo '{fechaDeOferta}'");
                if (!periodoOfertaOk) _output.WriteLine($"Error Período de Oferta: Esperaba '{fechaInicio}' - '{fechaFin}', veo '{periodoDeOferta}'");

                return clienteOk && metodoPagoOk && fechaOfertaOk && periodoOfertaOk;
            }
        
            catch (Exception ex)
            {
                _output.WriteLine("Excepción verificando detalles: " + ex.Message);
                return false;
            }
        }

        public bool CheckItem(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, tablaHerramientas);

        }

    }
}
