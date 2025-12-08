using AppForSEII2526.Web.API;


namespace AppForSEII2526.Web
{
    public class OfertaStateContainer
    {
        public OfertaDTO Oferta { get; private set; } = new OfertaDTO()
        {
            OfertaItems = new List<OfertaItemDTO>()
        };

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddHerramientaToOferta(HerramientaForOfertaDTO herramienta) // De momento, da error porque hay que actualizar los swager
        {
            //Comprobar antes que no esté ya en la lista
            if (!Oferta.OfertaItems.Any(oi => oi.Nombre == herramienta.Nombre && 
            oi.Fabricante == herramienta.Fabricante && oi.Material == herramienta.Material))
            {
                //añadimos la herramienta a la oferta
                Oferta.OfertaItems.Add(new OfertaItemDTO()
                {
                    Nombre = herramienta.Nombre,
                    Material = herramienta.Material,
                    Fabricante = herramienta.Fabricante,
                    PrecioOriginal = herramienta.Precio
                });
                NotifyStateChanged();
            }
        }

        //Borra una herramienta seleccionada de la oferta
        public void RemoveHerramientaFromOferta(OfertaItemDTO ofertaItem)
        {
            Oferta.OfertaItems.Remove(ofertaItem);
            NotifyStateChanged();
        }

        //Borra todas las herramientas seleccionadas de la oferta
        public void ClearOferta()
        {
            Oferta.OfertaItems.Clear();
            NotifyStateChanged();
        }

        public void OfertaProcessed()
        {
        //Hemos terminado el proceso de crear una oferta con datos
            Oferta = new OfertaDTO()
            {
                // Hemos terminado el proceso de crear una oferta sin datos
                OfertaItems = new List<OfertaItemDTO>(), // corrected capitalization
            };
        }
    }

}
*/
