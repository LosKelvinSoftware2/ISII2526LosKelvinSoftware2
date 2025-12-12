using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_CompraHerramienta
{
    public class UC_Compra_UIT : UC_UIT
    {
        private SelectHerramientaCompraPO _catalogoPO;
        private CreateCompraPO _formularioPO;
        private DetailsCompraPO _detallesPO;

        public UC_Compra_UIT(ITestOutputHelper output) : base(output)
        {
            _catalogoPO = new SelectHerramientaCompraPO(_driver, _output);
            _formularioPO = new CreateCompraPO(_driver, _output);
            _detallesPO = new DetailsCompraPO(_driver, _output);
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_Compra_Herramienta_Acero_Exito()
        {
            // Datos de nuestra base de datos

            // 1. Datos de Login 
            string usuarioLogin = "juan.perez@email.com"; 
            string passwordLogin = "Password123!";
            
            // 2. Datos de Búsqueda
            string materialBuscado = "Acero";
            string nombreHerramienta = "Taladro Percutor";
            string precioMaximo = ""; //Si se deja vacío, no se filtra por precio (coge todas las herramientas de ese material)

            // 3. Datos del Cliente (Usuario 1 de la base de datos)
            string nombreCliente = "Juan";
            string apellidoCliente = "Pérez";
            string direccionCliente = "Calle Mayor 10"; 
            string correoCliente = usuarioLogin;

            // --- ACT ---
            
            // 1. Abrir web y Login
            Initial_step_opening_the_web_page();
            Perform_login(usuarioLogin, passwordLogin);

            // 2. Navegar al catálogo
            _driver.Navigate().GoToUrl(_URI + "Compra/SelectHerramientaCompra");

            // 3. Buscar por MATERIAL (Acero)
            // Esto debería traer: Taladro Percutor, Martillo Neumático, Multiherramienta, Amoladora, etc.
            _catalogoPO.BuscarHerramienta(materialBuscado, precioMaximo);
            
            // 4. Añadir al carrito la herramienta específica
            _catalogoPO.AnadirAlCarrito(nombreHerramienta);

            // 5. Ir a tramitar
            _catalogoPO.IrATramitarCompra();

            // 6. Rellenar formulario y confirmar
            _formularioPO.RellenarFormulario(nombreCliente, apellidoCliente, correoCliente, direccionCliente, "Efectivo");
            _formularioPO.EnviarFormulario();
            _formularioPO.ConfirmarCompraEnModal(); 

            // --- ASSERT ---
            
            // 7. Verificar que en la pantalla final salen los datos correctos
            bool resultado = _detallesPO.VerificarDetallesCompra(nombreCliente, apellidoCliente, nombreHerramienta);
            
            Assert.True(resultado, $"Fallo en la verificación. Se esperaba cliente '{nombreCliente} {apellidoCliente}' y herramienta '{nombreHerramienta}'.");
        }



    }
}