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
        /* DATOS DE LA BASE DE DATOS PARA LAS PRUEBAS POR AL HACER UNA PRUEBA SE JODE, P vida
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

        private const string nombreHerramienta3 = "Esmeril de Banco";
        private const string Id3 = "26";
        private const string TotalPrecio3 = "95,00";

        private const string usuarioNombre = "Juan";
        private const string usuarioApellido = "Pérez";
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
           
            postPO.RellenarFormulario(usuarioNombre, usuarioApellido, usuarioTelefono, "Tarjeta");
            postPO.EstablecerCantidadItem(0, "1");


            DateTime fechaEntrega = DateTime.Now.AddDays(1); // siempreee Mañana
            postPO.EstablecerFechaEntrega(fechaEntrega);

            // Guardar
            // Esperamos a que el botón Submit del formulario sea visible (ID "Submit")
            selectPO.WaitForBeingVisible(By.Id("Submit"));
            postPO.SubmitReparacion();
            postPO.ConfirmDialog();


            // Paso 7: Verificar Detalles
            bool detallesCorrectos = detailsPO.VerificarDetallesReparacion(
                usuarioNombre + " " + usuarioApellido, // Ej: "Juan Pérez"
                "Tarjeta",
                "85,00" 
            );
            Assert.True(detallesCorrectos, "Los detalles de la reparación mostrados no son correctos");

            int diasReparacionHerramienta = 1; 

            string fechaRecogidaEsperada = CalcularFechaRecogidaEsperada(fechaEntrega, diasReparacionHerramienta);
            string fechaRecogidaReal = detailsPO.ObtenerFechaRecogidaTexto();

            Assert.Equal(fechaRecogidaEsperada, fechaRecogidaReal);

            // Verificar tabla
            var itemsReparados = new List<string[]> {
                new string[] { nombreHerramienta, "1", "85,00" }
            };
            Assert.True(detailsPO.CheckListaHerramientasReparadas(itemsReparados), "La lista de herramientas reparadas no coincide");
        }
        //axuliar para las mierdas de las fechas
        private string CalcularFechaRecogidaEsperada(DateTime fechaEntrega, int diasReparacion)
        {
            DateTime fechaRecogida = fechaEntrega;
            int diasAgregados = 0;

            // Para saltar Sábados y Domingos como en la api
            while (diasAgregados < diasReparacion)
            {
                fechaRecogida = fechaRecogida.AddDays(1);
                if (fechaRecogida.DayOfWeek != DayOfWeek.Saturday &&
                    fechaRecogida.DayOfWeek != DayOfWeek.Sunday)
                {
                    diasAgregados++;
                }
            }            
            return fechaRecogida.ToString("dd/MM/yyyy");
        }

        [Fact]
        public void FlujoAlternativo_0_FiltrarPorTiempoReparacion()
        {
            Initial_step_opening_the_web_page();
            InitialStepsForRepararHerramientas();

            string tiempoBusqueda = "5";

            selectPO.BuscarHerramientas("",tiempoBusqueda);

            // Verificar que aparece la herramienta correcta (Llave de Impacto, ID 25, 5 días)
            var listaEsperada = new List<string[]> {
            new string[] { nombreHerramienta2, "5" } 
            };

            Assert.True(selectPO.CheckListaHerramientas(listaEsperada),"El filtro por tiempo de reparación no devolvió los resultados esperados.");
        }

        [Fact]
        public void FlujoAlternativo_1_FiltrarHerramientas()
        {
            Initial_step_opening_the_web_page();
            InitialStepsForRepararHerramientas();

            // 1.1 Filtrar por nombre inexistente
            selectPO.BuscarHerramientas("HerramientaInexistente");
            
            bool listaFallida = _driver.FindElements(By.Id("TableOfHerramienta")).Count == 0;
            
            Assert.True(listaFallida, "La lista debería estar vacía para una herramienta inexistente");

            // 1.2 Filtrar por nombre correcto
            selectPO.BuscarHerramientas(nombreHerramienta2);
            var listaCorrecta = new List<string[]> { new string[] { nombreHerramienta2, "" } };
            Assert.True(selectPO.CheckListaHerramientas(listaCorrecta), "El filtro por nombre no funcionó correctamente");
        }

        [Fact]
        public void FlujoAlternativo_1_ValidacionFechaEntregaPasada()
        {
            Initial_step_opening_the_web_page();
            InitialStepsForRepararHerramientas();

            //añadir herramienta
            selectPO.BuscarHerramientas(nombreHerramienta);
            selectPO.AddHerramientaToReparacion(Id1);
            selectPO.ClickTramitarReparacion();

            //Rellenar
            postPO.RellenarFormulario(usuarioNombre, usuarioApellido, usuarioTelefono, "Tarjeta");

            //FECHA INVÁLIDA (Ayer)
            postPO.EstablecerFechaEntrega(DateTime.Now.AddDays(-1));

            //Intentar guardar
            postPO.SubmitReparacion();

            //Verificar
            Assert.True(postPO.HayErroresDeValidacion(""), "El sistema da error con fecha pasada");
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
            selectPO.RemoveHerramientaFromCart(nombreHerramienta2);

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
            postPO.RellenarFormulario("", "", "", "Tarjeta");

            postPO.SubmitReparacion();

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

            postPO.RellenarFormulario(usuarioNombre, usuarioApellido, usuarioTelefono, "Efectivo");

            // Poner cantidad a 0 (inválida)
            postPO.EstablecerCantidadItem(0, "0");

            postPO.SubmitReparacion();

            Assert.True(postPO.HayErroresDeValidacion(""), "Debe haber error de validación si la cantidad es 0");
        }

        ////////////////////////////////////////////////////////////Examen
        /// Comprobar en la BD que existen los datos 
        [Fact]
        public void Flujoexamen()
        {
            // Navegar
            Initial_step_opening_the_web_page();
            InitialStepsForRepararHerramientas();
            // Buscar y Añadir
            selectPO.BuscarHerramientas(nombreHerramienta2);
            selectPO.AddHerramientaToReparacion(Id2);

            selectPO.BuscarHerramientas(nombreHerramienta3);
            selectPO.AddHerramientaToReparacion(Id3);
            //ir al post
            selectPO.WaitForBeingVisible(By.Id("processBtn"));
            selectPO.ClickTramitarReparacion();

            //Volver al select
            postPO.WaitForBeingVisible(By.Id("ModifyTools"));
            postPO.modificar();

            //eliminar la primera herramienta
            selectPO.RemoveHerramientaFromCart(nombreHerramienta2);

            //ir al post
            selectPO.WaitForBeingVisible(By.Id("processBtn"));
            selectPO.ClickTramitarReparacion();


            postPO.RellenarFormulario(usuarioNombre, usuarioApellido, usuarioTelefono, "Tarjeta");
            postPO.EstablecerCantidadItem(0, "1");


            DateTime fechaEntrega = DateTime.Now.AddDays(1); // siempreee Mañana
            postPO.EstablecerFechaEntrega(fechaEntrega);

            // Guardar
            // Esperamos a que el botón Submit del formulario sea visible (ID "Submit")
            selectPO.WaitForBeingVisible(By.Id("Submit"));
            postPO.SubmitReparacion();
            postPO.ConfirmDialog();


            // Paso 7: Verificar Detalles
            bool detallesCorrectos = detailsPO.VerificarDetallesReparacion(
                usuarioNombre + " " + usuarioApellido, // Ej: "Juan Pérez"
                "Tarjeta",
                "95,00"
            );
            Assert.True(detallesCorrectos, "Los detalles de la reparación mostrados no son correctos");

            int diasReparacionHerramienta = 3;

            string fechaRecogidaEsperada = CalcularFechaRecogidaEsperada(fechaEntrega, diasReparacionHerramienta);
            string fechaRecogidaReal = detailsPO.ObtenerFechaRecogidaTexto();

            Assert.Equal(fechaRecogidaEsperada, fechaRecogidaReal);

            // Verificar tabla
            var itemsReparados = new List<string[]> {
                new string[] { nombreHerramienta3, "1", "95,00" }
            };
            Assert.True(detailsPO.CheckListaHerramientasReparadas(itemsReparados), "La lista de herramientas reparadas no coincide");
        }



    }
}