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
        private By _labelNombreCompleto = By.Id("NameSurname");
        private By _tablaItems = By.Id("Herramientas");

        public DetailsCompraPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) { }

        // Método simple para verificar texto
        public bool VerificarDetallesCompra(string nombreEsperado, string apellidoEsperado, string herramientaEsperada)
        {
            WaitForBeingVisible(_labelNombreCompleto);
            WaitForBeingVisible(_tablaItems);

            string textoNombre = _driver.FindElement(_labelNombreCompleto).Text;
            string textoTabla = _driver.FindElement(_tablaItems).Text;

            // Verificamos que el nombre completo esté correcto
            bool nombreOk = textoNombre.Contains(nombreEsperado) && textoNombre.Contains(apellidoEsperado);
            
            // Verificamos que la herramienta aparezca en la tabla
            bool herramientaOk = textoTabla.Contains(herramientaEsperada);

            if(!nombreOk) _output.WriteLine($"Fallo: Nombre esperado '{nombreEsperado} {apellidoEsperado}', encontrado '{textoNombre}'");
            if(!herramientaOk) _output.WriteLine($"Fallo: Herramienta '{herramientaEsperada}' no encontrada en la tabla.");

            return nombreOk && herramientaOk;
        }
    }
}