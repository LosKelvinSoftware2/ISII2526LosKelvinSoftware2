using AppForSEII2526.Web.API;
using System.Reflection;

namespace AppForSEII2526.Web
{
    public class CompraStateContainer
    {
        public CompraDTO Compra { get; private set; } = new CompraDTO()
        {
            CompraItems = new List<CompraItemDTO>(),
            FechaCompra = DateTime.Today.AddDays(1)
        };

        // El precio total ahora se calcula directo del DTO porque ya tiene el precio guardado
        public float PrecioTotal => Compra.CompraItems.Sum(ci => ci.Precio * ci.Cantidad);

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddHerramientaToCompra(CompraHerramientasDTO herramienta, int cantidad = 1, string descripcion = "")
        {
            if (!Compra.CompraItems.Any(ci => ci.HerramientaId == herramienta.Id))
            {
                var item = new CompraItemDTO
                {
                    HerramientaId = herramienta.Id,
                    Nombre = herramienta.Nombre,
                    Material = herramienta.Material,
                    Cantidad = cantidad,
                    Descripcion = descripcion,
                    Precio = herramienta.Precio
                };

                Compra.CompraItems.Add(item);
                NotifyStateChanged();
            }
        }

        public void RemoveItemFromCompra(CompraItemDTO item)
        {
            Compra.CompraItems.Remove(item);
            NotifyStateChanged();
        }

        //PARA DEVOLVER EL ID
        public int GetID(CompraDTO Compra)
        {
            foreach (var i in Compra.CompraItems)
            {
                return i.HerramientaId;
            }
            //Por si no hay nada en CompraItems
            throw new InvalidOperationException("No se encontró ningún item en la lista.");
        }

        // Limpiamos todo el carrito
        public void ClearCompraCart()
        {
            Compra.CompraItems.Clear();
            NotifyStateChanged();
        }

        // Finalizamos el proceso y reseteamos el estado
        public void CompraProcessed()
        {
            // Creamos un nuevo objeto limpio
            Compra = new CompraDTO()
            {
                CompraItems = new List<CompraItemDTO>(),
                FechaCompra = DateTime.Today.AddDays(1)
            };

            NotifyStateChanged();
        }
    }
}
