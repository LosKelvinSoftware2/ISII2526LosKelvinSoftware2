using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class AlquilerStateContainer
    {
        public AlquilerDTO Alquiler { get; private set; } = new AlquilerDTO()
        {
            AlquilarItems = new List<AlquilarItemDTO>()
        };
        public decimal PrecioTotal
        {
            get
            {
                int numberOfDays = (Alquiler.FechaFin - Alquiler.FechaInicio).Days;
                return Convert.ToDecimal(Alquiler.AlquilarItems.Sum(ri => ri.Precio * numberOfDays));
            }
        }
        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();



        public void AddHerramientaToAlquiler(AlquilerHerramientasDTO alquiler)
        {
            if (!Alquiler.AlquilarItems.Any(ri => ri.NombreHerramienta == alquiler.Nombre))
                Alquiler.AlquilarItems.Add(new AlquilarItemDTO()
                {
                    NombreHerramienta = alquiler.Nombre,
                    MaterialHerramienta = alquiler.Material,
                    Precio = alquiler.Precio,
                    Cantidad = 1
                }
            );

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
