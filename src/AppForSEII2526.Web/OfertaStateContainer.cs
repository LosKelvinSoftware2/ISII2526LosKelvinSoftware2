/*using AppForSEII2526.API.DTO.OfertaDTOs;
using AppForSEII2526.Web.API;


namespace AppForSEII2526.Web
{
    public class OfertaStateContainer
    {
        public OfertaDTO Oferta { get; private set; } = new OfertaDTO()
        {
            ofertaItems = new List<OfertaItemDTO>()
        };

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddHerramientaToOferta(HerramientaForOfertaDTO herramienta)
        {
            //Comprobar antes que no esté ya en la lista
            if (!Oferta.ofertaItems.Any(oi => oi.nombre == herramienta.Nombre && 
            oi.Fabricante == herramienta.fabricante && oi.material == herramienta.Material))
            {
                //añadimos la herramienta a la oferta
                Oferta.ofertaItems.Add(new OfertaItemDTO()
                {
                    nombre = herramienta.Nombre,
                    material = herramienta.Material,
                    Fabricante = herramienta.fabricante,
                    precioOriginal = herramienta.Precio
                });
            }
        }

        //Borra una herramienta seleccionada de la oferta
        public void RemoveHerramientaFromOferta(OfertaItemDTO ofertaItem)
        {
            Oferta.ofertaItems.Remove(ofertaItem);
            NotifyStateChanged();
        }

        //Borra todas las herramientas seleccionadas de la oferta
        public void ClearOferta()
        {
            Oferta.ofertaItems.Clear();
            NotifyStateChanged();
        }

        public void OfertaProcessed()
        {
        //Hemos terminado el proceso de crear una oferta con datos
            Oferta = new OfertaDTO()
            {
                // Hemos terminado el proceso de crear una oferta sin datos
                ofertaItems = new List<OfertaItemDTO>(),
            };
        }
    }

}
*/
