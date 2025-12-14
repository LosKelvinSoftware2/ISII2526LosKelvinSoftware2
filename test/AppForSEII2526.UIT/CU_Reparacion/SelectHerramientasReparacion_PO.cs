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

        public void BuscarHerramientas(string nombre, string dias = "")
        {
            WaitForBeingClickable(inputNombre);
            _driver.FindElement(inputNombre).Clear();
            _driver.FindElement(inputNombre).SendKeys(nombre);

            if (!string.IsNullOrEmpty(dias))
            {
                _driver.FindElement(inputDias).Clear();
                _driver.FindElement(inputDias).SendKeys(dias);
            }
            _driver.FindElement(buttonBuscar).Click();
            try
            {
                WaitForBeingVisible(tablaHerramientas);
            }
            catch
            {
                /* Puede no aparecer si no hay resultados */
            }
        }

        
        public void AddHerramientaToReparacion(string idHerramienta)
        {
            // ID en Razor: id="BtnAdd_@h.Id"
            // Debes pasar el ID numérico (ej: "1") desde el test
            By btnAdd = By.Id($"BtnAdd_{idHerramienta}");
            WaitForBeingVisible(btnAdd);
            WaitForBeingClickable(btnAdd);
            _driver.FindElement(btnAdd).Click();
        }

        public void RemoveHerramientaFromCart(string nombreHerramienta)
        {
            
            By btnRemove = By.Id($"removeHerramienta_{nombreHerramienta}");
            WaitForBeingVisible(btnRemove);
            _driver.FindElement(btnRemove).Click();
        }

        public void ClickTramitarReparacion()
        {
            WaitForBeingVisible(buttonTramitar);
            _driver.FindElement(buttonTramitar).Click();
        }

        public bool IsTramitarButtonVisible()
        {
            try
            {
                
                // Si el botón está oculto o deshabilitado, esto saltará o devolverá false
                var btn = _driver.FindElement(buttonTramitar);
                return btn.Displayed && btn.Enabled;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool CheckListaHerramientas(List<string[]> herramientasEsperadas)
        {
            try
            {
                WaitForBeingVisible(tablaHerramientas);
                string textoTabla = _driver.FindElement(tablaHerramientas).Text;

                foreach (var h in herramientasEsperadas)
                {
                    // h[0] = Nombre, h[1] = Material, h[2] = Precio (según lo que envíes en el test)
                    if (!textoTabla.Contains(h[0]) || !textoTabla.Contains(h[1]))
                    {
                        _output.WriteLine($"Falta en tabla: {h[0]} o {h[1]}");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckMessageError(string errorMessage)
        {
            try
            {
                WaitForBeingVisible(errorShownBy);
                string textoActual = _driver.FindElement(errorShownBy).Text;
                return textoActual.Contains(errorMessage);
            }
            catch (WebDriverTimeoutException)
            {
                return false; // No apareció el mensaje
            }
        }
    }
}