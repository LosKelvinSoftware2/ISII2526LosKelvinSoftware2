using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Reparacion
{
    public class SelectHerramientasReparacion_PO : PageObject
    {
        // Localizadores basados en SelectHerramientaReparacion.razor
        private By inputNombre = By.Id("inputTitle"); 
        private By inputDias = By.Id("inputGenre");
        private By buttonBuscar = By.Id("searchHerramientas"); 
        private By tablaHerramientas = By.Id("TableOfHerramienta"); 
        private By errorShownBy = By.Id("ErrorsShown"); 
        private By buttonTramitar = By.Id("processBtn"); 

        public SelectHerramientasReparacion_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void BuscarHerramientas(string nombre, string dias)
        {
            WaitForBeingClickable(inputNombre);
            // Limpiamos los campos antes de escribir
            _driver.FindElement(inputNombre).Clear();
            _driver.FindElement(inputDias).Clear();

            _driver.FindElement(inputNombre).SendKeys(nombre);
            _driver.FindElement(inputDias).SendKeys(dias);
            _driver.FindElement(buttonBuscar).Click();
        }

        public bool CheckListaHerramientas(List<string[]> herramientasEsperadas)
        {
            // Usa el método genérico CheckBodyTable heredado de PageObject
            return CheckBodyTable(herramientasEsperadas, tablaHerramientas);
        }

        public void AnadirHerramientaAReparacion(string idHerramienta)
        {
            By btnAdd = By.Id($"BtnAdd_{idHerramienta}");
            WaitForBeingClickable(btnAdd);
            _driver.FindElement(btnAdd).Click();
        }

        public void EliminarHerramientaDelCarrito(string nombreHerramienta)
        {
            By btnRemove = By.Id($"removeHerramienta_{nombreHerramienta}");
            WaitForBeingClickable(btnRemove);
            _driver.FindElement(btnRemove).Click();
        }

        public void ClicTramitarReparacion()
        {
            WaitForBeingClickable(buttonTramitar);
            _driver.FindElement(buttonTramitar).Click();
        }

        // Para verificar si el botón está visible (Caso de prueba de carrito vacío)
        public bool EsVisibleBotonTramitar()
        {
            try
            {
                return _driver.FindElement(buttonTramitar).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool CheckMensajeError(string mensajeError)
        {
            WaitForBeingVisible(errorShownBy);
            IWebElement errorActual = _driver.FindElement(errorShownBy);
            _output.WriteLine($"Mensaje de error encontrado: {errorActual.Text}");
            return errorActual.Text.Contains(mensajeError);
        }
    }
}