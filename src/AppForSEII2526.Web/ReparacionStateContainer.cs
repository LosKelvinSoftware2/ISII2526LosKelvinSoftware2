using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class ReparacionStateContainer
    {
        public ReparacionDTO Reparacion { get; private set; } = new ReparacionDTO()
        {
            ItemsReparacion = new List<ReparacionItemDTO>(),
            FechaEntrega = DateTime.Today.AddDays(1)
        };

        // El precio total ahora se calcula directo del DTO porque ya tiene el precio guardado
        public float PrecioTotal => Reparacion.ItemsReparacion.Sum(ri => ri.Precio * ri.Cantidad);

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddHerramientaToReparacion(HerramientaRepaDTO herramienta, int cantidad = 1, string descripcion = "")
        {
            if (!Reparacion.ItemsReparacion.Any(ri => ri.HerramientaId == herramienta.Id))
            {
                var item = new ReparacionItemDTO
                {
                    HerramientaId = herramienta.Id,
                    Cantidad = cantidad,
                    Descripcion = descripcion,
                    NombreHerramienta = herramienta.Nombre,
                    Precio = herramienta.Precio
                };

                Reparacion.ItemsReparacion.Add(item);
                NotifyStateChanged();
            }
        }

        public void RemoveItemFromReparacion(ReparacionItemDTO item)
        {
            Reparacion.ItemsReparacion.Remove(item);
            NotifyStateChanged();
        }

        // Limpiamos todo el carrito
        public void ClearReparacionCart()
        {
            Reparacion.ItemsReparacion.Clear();
            NotifyStateChanged();
        }

        // Finalizamos el proceso y reseteamos el estado
        public void ReparacionProcessed()
        {
            // Creamos un nuevo objeto limpio
            Reparacion = new ReparacionDTO()
            {
                ItemsReparacion = new List<ReparacionItemDTO>(),
                FechaEntrega = DateTime.Today.AddDays(1)
            };

            NotifyStateChanged();
        }
    }
}