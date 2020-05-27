namespace GameStore.Web.Models.ViewModels.GameViewModels
{
    public class GameLocalizationViewModel
    {
        public string Id { get; set; }
        public string GameId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string QuantityPerUnit { get; set; }
        public string CultureName { get; set; }

        public bool IsAssigned()
        {
            var isAssigned =
                Name != default ||
                Description != default ||
                QuantityPerUnit != default;

            return isAssigned;
        }
    }
}