using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.Web.Models.ViewModels.GameViewModels;
using GameStore.Web.Models.ViewModels.ImageViewModels;

namespace GameStore.Web.Mapping.Converters
{
    public class GameImageDtoToGameImageViewModelConverter : ITypeConverter<GameImageDto, GameImageViewModel>
    {
        public GameImageViewModel Convert(GameImageDto source, GameImageViewModel destination, ResolutionContext context)
        {
            const string imageSrcSegment = "data:image;base64,";
            var viewModel = new GameImageViewModel
            {
                Id = source.Id, 
                GameKey = source.GameKey,
                Content = $"{imageSrcSegment}{System.Convert.ToBase64String(source.Content)}"
            };

            return viewModel;
        }
    }
}