using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class AlquilerStateContainer
    {
        public AlquilerDTO Alquiler { get; private set; } = new AlquilerDTO()
        {
            AlquilarItems = new List<AlquilarItemDTO>()
        };
        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();



        public void AddHerramientaToAlquiler(AlquilerHerramientasDTO alquiler)
        {
            if (!Alquiler.AlquilarItems.Any(ri => ri.NombreHerramienta == alquiler.Nombre))
            {

                Alquiler.AlquilarItems.Add(new AlquilarItemDTO()
                {
                    NombreHerramienta = alquiler.Nombre,
                    MaterialHerramienta = alquiler.Material,
                    Precio = alquiler.Precio,
                    Cantidad = 1
                }
            );
            } else
            {
                foreach (var herramienta in Alquiler.AlquilarItems) {
                    if (herramienta.NombreHerramienta == alquiler.Nombre && herramienta.MaterialHerramienta == alquiler.Material)
                        herramienta.Cantidad = herramienta.Cantidad + 1;
                }
            }
            NotifyStateChanged();
        }
        public void RemoveAlquilarItemToAlquiler(AlquilarItemDTO item)
        {
            Alquiler.AlquilarItems.Remove(item);
        }

        public void ClearRentingCart()
        {
            Alquiler.AlquilarItems.Clear();
        }
        public void RentalProcessed()
        {
            Alquiler = new AlquilerDTO()
            {
                AlquilarItems = new List<AlquilarItemDTO>()
            };
        }
    }
}
