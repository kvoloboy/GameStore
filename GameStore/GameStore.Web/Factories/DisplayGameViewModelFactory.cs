using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Models;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.CommentViewModels;
using GameStore.Web.Models.ViewModels.GameViewModels;
using Microsoft.Extensions.Localization;

namespace GameStore.Web.Factories
{
    public class DisplayGameViewModelFactory : IAsyncViewModelFactory<GameDto, DisplayGameViewModel>
    {
        private readonly IGameService _gameService;
        private readonly IAsyncViewModelFactory<CommentViewModel, DisplayCommentsViewModel> _displayCommentViewModelFactory;
        private readonly IStringLocalizer<DisplayGameViewModelFactory> _stringLocalizer;
        private readonly IMapper _mapper;

        public DisplayGameViewModelFactory(
            IGameService gameService,
            IAsyncViewModelFactory<CommentViewModel, DisplayCommentsViewModel> displayCommentViewModelFactory,
            IStringLocalizer<DisplayGameViewModelFactory> stringLocalizer,
            IMapper mapper)
        {
            _gameService = gameService;
            _displayCommentViewModelFactory = displayCommentViewModelFactory;
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
        }

        public async Task<DisplayGameViewModel> CreateAsync(GameDto model)
        {
            var displayGameViewModel = GetDisplayViewModel(model);
            var commentViewModel = new CommentViewModel {GameKey = model.Key};
            var displayCommentsViewModel = await _displayCommentViewModelFactory.CreateAsync(commentViewModel);

            displayGameViewModel.DisplayCommentsViewModel = displayCommentsViewModel;
            displayGameViewModel.CommentsCount = GetCommentsCount(displayCommentsViewModel.Comments);

            return displayGameViewModel;
        }

        private DisplayGameViewModel GetDisplayViewModel(GameDto gameDto)
        {
            var defaultValue = _stringLocalizer["None"];
            var viewModel = _mapper.Map<DisplayGameViewModel>(gameDto);
            viewModel.Price = _gameService.ComputePriceWithDiscount(gameDto.Price, gameDto.Discount);
            viewModel.Rating = _mapper.Map<RatingViewModel>(gameDto.RatingDto);
            SetupDefaultValue(viewModel, defaultValue);

            return viewModel;
        }

        private static void SetupDefaultValue(DisplayGameViewModel viewModel, string defaultValue)
        {
            if (viewModel.Genres == null || !viewModel.Genres.Any())
            {
                viewModel.Genres = new[] {defaultValue};
            }

            if (viewModel.Platforms == null || !viewModel.Platforms.Any())
            {
                viewModel.Platforms = new[] {defaultValue};
            }
        }

        private static int GetCommentsCount(IEnumerable<CommentViewModel> comments)
        {
            var aggregator = comments.Count();

            foreach (var comment in comments)
            {
                ComputeCommentsCount(comment.Children, ref aggregator);
            }

            return aggregator;
        }

        private static void ComputeCommentsCount(IEnumerable<CommentViewModel> comments, ref int aggregator)
        {
            aggregator += comments.Count();

            foreach (var comment in comments)
            {
                ComputeCommentsCount(comment.Children, ref aggregator);
            }
        }
    }
}