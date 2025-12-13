using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace AppForSEII2526.UIT.CU_Alquiler
{
    public class DetailsAlquiler_PO : PageObject
    {
        By nombreCliente = By.Id("NombreCliente");
        By direccion = By.Id("Direccion");
        By fechaAlquiler = By.Id("FechaAlquiler");
        By precio = By.Id("Precio");
        By fechaInicio = By.Id("FechaInicio");
        By fechaFin = By.Id("FechaFin");
        By tablaHerramientas = By.Id("HerramientasAlquiladas");
        public DetailsAlquiler_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public bool CheckDatos(string nombreApellido, string direccionEnv, string fechaDelAlquiler, string precioTotal, string fechaDeInicio, string fechaDeFin)
        {
            WaitForBeingVisible(nombreCliente);
            WaitForBeingVisible(direccion);
            WaitForBeingVisible(fechaAlquiler);
            WaitForBeingVisible(precio);
            WaitForBeingVisible(fechaInicio);
            WaitForBeingVisible(fechaFin);

            IWebElement name = _driver.FindElement(nombreCliente);
            _output.WriteLine($"Nombre y apellido: {name.Text}");
            IWebElement direc = _driver.FindElement(direccion);
            _output.WriteLine($"Dirección: {direc.Text}");
            IWebElement fechaAl = _driver.FindElement(fechaAlquiler);
            _output.WriteLine($"Fecha alquiler: {fechaAl.Text}");
            IWebElement prec = _driver.FindElement(precio);
            _output.WriteLine($"Precio: {prec.Text}");
            IWebElement fechaIn = _driver.FindElement(fechaInicio);
            _output.WriteLine($"Fecha inicio: {fechaIn.Text}");
            IWebElement fechaFi = _driver.FindElement(fechaFin);
            _output.WriteLine($"Fecha fin: {fechaFi.Text}");

            return name.Text.Contains(nombreApellido) && direc.Text.Contains(direccionEnv) &&
                fechaAl.Text.Contains(fechaDelAlquiler) && prec.Text.Contains(precioTotal) &&
                fechaIn.Text.Contains(fechaDeInicio) && fechaFi.Text.Contains(fechaDeFin);
        }
        public bool CheckTablaHerramientas(List<string[]> expectedHerramientas)
        {
            return CheckBodyTable(expectedHerramientas, tablaHerramientas);
        }
    }
}
