using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models;
using GameStore.Web.Models.ViewModels.GameViewModels;
using GameStore.Web.Models.ViewModels.PublisherViewModels;

namespace GameStore.Web.Factories
{
    public class GameViewModelFactory : IAsyncViewModelFactory<ModifyGameViewModel, GameViewModel>
    {
        private readonly IGenreService _genreServices;
        private readonly IPlatformService _platformServices;
        private readonly IPublisherService _publisherServices;
        private readonly IMapper _mapper;

        public GameViewModelFactory(
            IGenreService genreServices,
            IPlatformService platformServices,
            IPublisherService publisherServices,
            IMapper mapper)
        {
            _genreServices = genreServices;
            _platformServices = platformServices;
            _publisherServices = publisherServices;
            _mapper = mapper;
        }

        public async Task<GameViewModel> CreateAsync(ModifyGameViewModel model)
        {
            var gameSelectionDataViewModel = new GameSelectionDataViewModel
            {
                Genres = await CreateGenresAsync(),
                Platforms = await CreatePlatformsAsync(),
                Publishers = await CreatePublishersAsync()
            };

            var viewModel = new GameViewModel
            {
                ModifyGameViewModel = model,
                GameSelectionDataViewModel = gameSelectionDataViewModel
            };

            return viewModel;
        }

        private async Task<IEnumerable<TreeViewListItem>> CreateGenresAsync()
        {
            var genreNodeTree = await _genreServices.GetGenresTreeAsync();
            var genreListItems = _mapper.Map<IEnumerable<TreeViewListItem>>(genreNodeTree);

            return genreListItems;
        }

        private async Task<IEnumerable<ListItem>> CreatePlatformsAsync()
        {
            var platformsDto = await _platformServices.GetAllAsync();
            var platforms = _mapper.Map<IEnumerable<ListItem>>(platformsDto);

            return platforms;
        }

        private async Task<IEnumerable<PublisherViewModel>> CreatePublishersAsync()
        {
            var publishersDto = await _publisherServices.GetAllAsync(Culture.Current);
            var publishers = _mapper.Map<IEnumerable<PublisherViewModel>>(publishersDto);

            return publishers;
        }
    }
}