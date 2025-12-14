using AppForSEII2526.UIT.Shared;
using Xunit;
using Xunit.Abstractions;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AppForSEII2526.UIT.UC_Reparacion
{
    public class UC_RepararHerramientas_UIT : UC_UIT
    {
        private SelectHerramientasReparacion_PO selectPO;
        private PostReparacion_PO postPO;
        private DetailsReparacion_PO detailsPO;
        /* DATOS DE LA BASE DE DATOS PARA LAS PRUEBAS POR AL HACER UNA PRUEBA SE JODE P vida
            (21, 'Soldador Inverter', 'Metal', 220.00, 4, 4),
            (22, 'Aspirador Industrial', 'Plástico', 180.50, 3, 2),
            (23, 'Mezcladora de Mortero', 'Acero', 115.00, 2, 18),
            (24, 'Gato Hidráulico', 'Hierro', 85.00, 1, 20),
            (25, 'Llave de Impacto', 'Aluminio', 260.00, 5, 9),
            (26, 'Esmeril de Banco', 'Hierro', 95.00, 3, 7),
            (27, 'Cortasetos Eléctrico', 'Plástico', 75.00, 2, 8),
            (28, 'Motosierra Gasolina', 'Acero', 350.00, 6, 1),
            (29, 'Polipasto Eléctrico', 'Acero', 190.00, 4, 20),
            (30, 'Pistola de Pintar', 'Plástico', 55.00, 2, 15);
         */

        // Datos de Prueba
        private const string nombreHerramienta = "Gato Hidráulico";
        private const string Id1 = "24";
        private const string TotalPrecio = "85.00"; 

        private const string nombreHerramienta2 = "Llave de Impacto";
        private const string Id2 = "25";
        private const string TotalPrecio2 = "260.00";

        private const string usuarioNombre = "Juan";
        private const string usuarioApellido = "Pérez";
        private const string usuarioUsername = "juanperez";
        private const string usuarioEmail = "juan.perez@email.com";
        private const string usuarioTelefono = "+34600111222";

        public UC_RepararHerramientas_UIT(ITestOutputHelper output) : base(output)
        {
            selectPO = new SelectHerramientasReparacion_PO(_driver, _output);
            postPO = new PostReparacion_PO(_driver, _output);
            detailsPO = new DetailsReparacion_PO(_driver, _output);
        }

        private void InitialStepsForRepararHerramientas()
        {
            selectPO.WaitForBeingVisible(By.Id("CreateReparacion"));
            _driver.FindElement(By.Id("CreateReparacion")).Click();            
        }

        [Fact]
        public void FlujoBasico_RepararHerramientas_OK()
        {
            // Navegar
            Initial_step_opening_the_web_page();
            InitialStepsForRepararHerramientas();
            // Buscar y Añadir
            selectPO.BuscarHerramientas(nombreHerramienta);
            selectPO.AddHerramientaToReparacion(Id1);

            selectPO.WaitForBeingVisible(By.Id("processBtn"));             
            selectPO.ClickTramitarReparacion();
           
            postPO.RellenarFormulario(usuarioNombre, usuarioApellido, usuarioUsername, usuarioTelefono, "Tarjeta");
            postPO.EstablecerCantidadItem(0, "1");

            // Guardar
            // Esperamos a que el botón Submit del formulario sea visible (ID "Submit")
            selectPO.WaitForBeingVisible(By.Id("Submit"));
            postPO.SubmitReparacion();
            postPO.ConfirmDialog();


            // Paso 7: Verificar Detalles
            // CORRECCIÓN: La vista DetailsReparacion muestra "@repa.NombreCliente @repa.ApellidosCliente"
            // No incluye el username en el nombre completo visualizado.
            bool detallesCorrectos = detailsPO.VerificarDetallesReparacion(
                usuarioNombre + " " + usuarioApellido, // Ej: "Juan Pérez"
                "Tarjeta",
                "85,00" // Asegúrate de que este precio coincida con tu BD
            );
            Assert.True(detallesCorrectos, "Los detalles de la reparación mostrados no son correctos");

            // Verificar tabla
            var itemsReparados = new List<string[]> {
        new string[] { nombreHerramienta, "1", "85,00" }
    };
            Assert.True(detailsPO.CheckListaHerramientasReparadas(itemsReparados), "La lista de herramientas reparadas no coincide");
        }

        [Fact]
        public void FlujoAlternativo_1_FiltrarHerramientas()
        {
            Initial_step_opening_the_web_page();
            InitialStepsForRepararHerramientas();

            // 1.1 Filtrar por nombre inexistente
            selectPO.BuscarHerramientas("HerramientaInexistente");
            // Aquí podríamos comprobar que la tabla está vacía o no contiene el elemento, 
            // pero el PO CheckListaHerramientas devuelve false si no encuentra lo esperado.
            bool listaFallida = _driver.FindElements(By.Id("TableOfHerramienta")).Count == 0;
            //Assert.True(listaFallida);
            // Verificación corregida usando el PO y el ID correcto
            Assert.True(listaFallida, "La lista debería estar vacía para una herramienta inexistente");

            // 1.2 Filtrar por nombre correcto
            selectPO.BuscarHerramientas(nombreHerramienta2);
            var listaCorrecta = new List<string[]> { new string[] { nombreHerramienta2, "" } };
            Assert.True(selectPO.CheckListaHerramientas(listaCorrecta), "El filtro por nombre no funcionó correctamente");
        }

        [Fact]
        public void FlujoAlternativo_2_y_3_ModificarCarrito_VaciarCarrito()
        {
            Initial_step_opening_the_web_page();
            InitialStepsForRepararHerramientas();

            // Añadir herramienta
            selectPO.BuscarHerramientas(nombreHerramienta2);
            selectPO.AddHerramientaToReparacion(Id2);
            selectPO.WaitForBeingVisible(By.Id("processBtn"));
            Assert.True(selectPO.IsTramitarButtonVisible(), "El botón tramitar debería aparecer al añadir item");

            // Flujo Alternativo 2: Modificar carrito (Borrar herramienta)
            // El PO RemoveHerramientaFromCart usa el nombre para el ID del botón remove
            selectPO.RemoveHerramientaFromCart(nombreHerramienta2);
            //QUIERO QUE SE ESPERE 5 SEGUNDOS PARA VER EL CAMBIO
            System.Threading.Thread.Sleep(5000);
            // Flujo Alternativo 3: Carrito vacío -> No se puede continuar
            Assert.False(selectPO.IsTramitarButtonVisible(), "El botón tramitar NO debería ser visible/activo si el carrito está vacío");
        }

        [Fact]
        public void FlujoAlternativo_3_CarritoVacio_BotonDeshabilitado()
        {
            Initial_step_opening_the_web_page();
            InitialStepsForRepararHerramientas();

            Assert.False(selectPO.IsTramitarButtonVisible(), "El botón Tramitar no debería estar activo si el carrito está vacío.");
        }

        [Fact]
        public void FlujoAlternativo_4_ValidacionDatosObligatorios()
        {
            Initial_step_opening_the_web_page();
            InitialStepsForRepararHerramientas();

            // Preparar escenario: llegar al formulario
            selectPO.BuscarHerramientas(nombreHerramienta2);
            selectPO.AddHerramientaToReparacion(Id2);
            selectPO.ClickTramitarReparacion();

            // Dejar campos obligatorios vacíos (Nombre, Apellido, etc.)
            postPO.RellenarFormulario("", "", "", "", "Tarjeta");

            postPO.SubmitReparacion();

            // Si hay validación de cliente, no debería salir el modal, 
            // pero si sale el modal y damos OK, el backend o el form validator deberían saltar.
            // El PO "ConfirmDialog" asume que el dialogo sale. Si la validación es HTML5/EditForm antes del submit real:
            // Intentamos verificar errores.

            // NOTA: Si el botón es type="submit", el OnValidSubmit solo salta si es válido.
            // Si es inválido, EditForm muestra errores.

            bool hayErrores = postPO.HayErroresDeValidacion("The Nombre field is required") ||
                              postPO.HayErroresDeValidacion("required");

            Assert.True(hayErrores, "El sistema debería mostrar errores de validación ante campos vacíos");
        }

        [Fact]
        public void FlujoAlternativo_5_ValidacionCantidadCero()
        {
            Initial_step_opening_the_web_page();
            InitialStepsForRepararHerramientas();

            selectPO.BuscarHerramientas(nombreHerramienta2);
            selectPO.AddHerramientaToReparacion(Id2);
            selectPO.ClickTramitarReparacion();

            postPO.RellenarFormulario(usuarioNombre, usuarioApellido, usuarioUsername, usuarioTelefono, "Efectivo");

            // Poner cantidad a 0 (inválida)
            postPO.EstablecerCantidadItem(0, "0");

            postPO.SubmitReparacion();

            // Al ser OnValidSubmit, si la validación falla, no abre el dialogo y muestra errores en el Summary
            // O si abre dialogo (depende implementación Razor), al volver falla.
            // Asumimos comportamiento estándar de Blazor EditForm: ValidationSummary aparece.

            Assert.True(postPO.HayErroresDeValidacion(""), "Debe haber error de validación si la cantidad es 0");
        }



    }
}