using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.ImageViewModels;

namespace GameStore.Web.Factories
{
    public class GameImageViewModelFactory : IViewModelFactory<string, GameImageViewModel>
    {
        private const string DefaultImagePath = "/img/Game/no-image.png";
        public GameImageViewModel Create(string model)
        {
            var viewModel = new GameImageViewModel
            {
                GameKey = model,
                Content = DefaultImagePath
            };

            return viewModel;
        }
    }
}