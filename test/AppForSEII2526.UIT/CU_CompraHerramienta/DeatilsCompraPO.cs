using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace AppForSEII2526.UIT.CU_CompraHerramienta
{
    public class DetailsCompraPO : PageObject
    {
        // IDs extraídos de DetailsCompra.razor
        private By _labelNombreCompleto = By.Id("NameSurname");
        private By _labelDireccion = By.Id("DeliveryAddress");
        private By _labelPrecioTotal = By.Id("TotalPrice");
        private By _tablaItems = By.Id("Herramientas");

        public DetailsCompraPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) { }

        // --- VERIFICACIONES ---

        public bool VerificarDetallesCompra(string nombre, string apellido, string direccionEsperada, string herramientaEsperada)
        {
            try
            {
                // Esperamos que todos los elementos sean visibles
                WaitForBeingVisible(_labelNombreCompleto);
                WaitForBeingVisible(_labelDireccion);
                WaitForBeingVisible(_tablaItems);

                string textoNombre = _driver.FindElement(_labelNombreCompleto).Text;
                string textoDireccion = _driver.FindElement(_labelDireccion).Text;
                string textoTabla = _driver.FindElement(_tablaItems).Text;

                // Validaciones
                bool nombreOk = textoNombre.Contains(nombre) && textoNombre.Contains(apellido);
                bool direccionOk = textoDireccion.Equals(direccionEsperada);
                bool herramientaOk = textoTabla.Contains(herramientaEsperada);

                // Logs de error para saber qué falló
                if (!nombreOk) _output.WriteLine($"Error Nombre: Esperaba '{nombre} {apellido}', veo '{textoNombre}'");
                if (!direccionOk) _output.WriteLine($"Error Dirección: Esperaba '{direccionEsperada}', veo '{textoDireccion}'");
                if (!herramientaOk) _output.WriteLine($"Error Herramienta: No veo '{herramientaEsperada}' en la tabla");

                return nombreOk && direccionOk && herramientaOk;
            }
            catch (Exception ex)
            {
                _output.WriteLine("Excepción verificando detalles: " + ex.Message);
                return false;
            }
        }

        public bool CheckPrecioTotal(string precioEsperado)
        {
            WaitForBeingVisible(_labelPrecioTotal);
            string textoPrecio = _driver.FindElement(_labelPrecioTotal).Text;
            return textoPrecio.Contains(precioEsperado);
        }
    }
}