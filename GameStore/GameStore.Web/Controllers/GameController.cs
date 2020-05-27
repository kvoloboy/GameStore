using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Models;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Models;
using GameStore.Core.Models.Identity;
using GameStore.Identity.Attributes;
using GameStore.Identity.Extensions;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Filters;
using GameStore.Web.Models.ViewModels.FilterViewModels;
using GameStore.Web.Models.ViewModels.GameViewModels;
using GameStore.Web.Models.ViewModels.ImageViewModels;
using GameStore.Web.Models.ViewModels.PageViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameStore.Web.Controllers
{
    [Route("/games")]
    public class GameController : Controller
    {
        private readonly IAsyncViewModelFactory<ModifyGameViewModel, GameViewModel> _gameViewModelFactory;
        private readonly IAsyncViewModelFactory<FilterSelectedOptionsViewModel, FilterViewModel> _filterViewModelFactory;
        private readonly IViewModelFactory<PageOptions, PageOptionsViewModel> _pageOptionsViewModelFactory;
        private readonly IViewModelFactory<string, GameImageViewModel> _gameImageViewModelFactory;
        private readonly IGameService _gameService;
        private readonly IRatingService _ratingService;
        private readonly ILogger<GameController> _logger;
        private readonly IMapper _mapper;
        private readonly IAsyncViewModelFactory<GameDto, DisplayGameViewModel> _displayGameViewModelFactory;

        public GameController(
            IAsyncViewModelFactory<ModifyGameViewModel, GameViewModel> gameViewModelFactory,
            IAsyncViewModelFactory<GameDto, DisplayGameViewModel> displayGameViewModelFactory,
            IAsyncViewModelFactory<FilterSelectedOptionsViewModel, FilterViewModel> filterViewModelFactory,
            IViewModelFactory<PageOptions, PageOptionsViewModel> pageOptionsViewModelFactory,
            IViewModelFactory<string, GameImageViewModel> gameImageViewModelFactory,
            IGameService gameService,
            IRatingService ratingService,
            ILogger<GameController> logger,
            IMapper mapper)
        {
            _gameService = gameService;
            _ratingService = ratingService;
            _gameViewModelFactory = gameViewModelFactory;
            _displayGameViewModelFactory = displayGameViewModelFactory;
            _filterViewModelFactory = filterViewModelFactory;
            _pageOptionsViewModelFactory = pageOptionsViewModelFactory;
            _gameImageViewModelFactory = gameImageViewModelFactory;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> GetGameAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return NotFound();
            }

            var gameDto = await _gameService.GetByKeyAsync(key);
            var viewModel = await _displayGameViewModelFactory.CreateAsync(gameDto);
            await _gameService.IncrementVisitsCountAsync(key);
            
            if (!viewModel.Images.Any())
            {
                var defaultImage = _gameImageViewModelFactory.Create(key);
                viewModel.Images.Add(defaultImage);
            }

            await _gameService.IncrementVisitsCountAsync(key);
            _logger.LogDebug($"The game with key {key} was read from database");

            return View("Details", viewModel);
        }

        [HttpGet("rate")]
        [HasPermission(Permissions.RateGame)]
        public async Task<IActionResult> SetRatingAsync(string gameId, int value)
        {
            var userId = User?.GetId();
            await _ratingService.CreateOrUpdateAsync(gameId, userId, value);

            return RedirectToAction(nameof(GetRatingAsync), new {gameId});
        }

        [HttpGet("rating")]
        public async Task<IActionResult> GetRatingAsync(string gameId)
        {
            var ratingDto = await _ratingService.GetForGameAsync(gameId);
            var viewModel = _mapper.Map<RatingViewModel>(ratingDto);

            return PartialView("Stars", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(FilterSelectedOptionsViewModel selectedOptionsViewModel)
        {
            var pageList = await GetPageListAsync(selectedOptionsViewModel);
            var catalogue = GetCatalogueGameViewModel(pageList);
            var pageViewModel = new PageListViewModel
            {
                Catalogue = catalogue,
                Filter = await GetFilterViewModelAsync(selectedOptionsViewModel, pageList)
            };

            return View("Index", pageViewModel);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredAsync(FilterSelectedOptionsViewModel filterSelectedOptionsViewModel)
        {
            var pageList = await GetPageListAsync(filterSelectedOptionsViewModel);
            var catalogue = GetCatalogueGameViewModel(pageList);

            return PartialView("Catalogue", catalogue);
        }

        [HttpGet("deleted")]
        [HasPermission(Permissions.ReadDeletedGames)]
        public IActionResult GetDeleted(FilterSelectedOptionsViewModel selectedOptionsViewModel)
        {
            selectedOptionsViewModel.IsDeleted = true;

            return RedirectToAction(nameof(GetAllAsync), selectedOptionsViewModel);
        }

        [HttpGet("new")]
        [HasPermission(Permissions.CreateGame)]
        public async Task<IActionResult> CreateAsync()
        {
            var viewModel = await _gameViewModelFactory.CreateAsync(new ModifyGameViewModel());

            return View("Create", viewModel);
        }

        [HttpPost("new")]
        [HasPermission(Permissions.CreateGame)]
        public async Task<IActionResult> CreateAsync(ModifyGameViewModel modifyGameViewModel)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = await _gameViewModelFactory.CreateAsync(modifyGameViewModel);

                return View("Create", viewModel);
            }

            var dto = _mapper.Map<ModifyGameDto>(modifyGameViewModel);

            try
            {
                await _gameService.CreateAsync(dto);
            }
            catch (ValidationException<GameRoot> e)
            {
                const string prefix = nameof(ModifyGameViewModel);
                ModelState.AddModelError($"{prefix}.{e.Key}", e.Message);

                var viewModel = await _gameViewModelFactory.CreateAsync(modifyGameViewModel);
                _logger.LogWarning(e.Message);

                return View("Create", viewModel);
            }

            _logger.LogDebug($"A new game with name: {modifyGameViewModel.Name} was added to database");

            return RedirectToAction(nameof(GetAllAsync));
        }

        [HttpGet("update")]
        [IsAllowedUpdateGame]
        public async Task<IActionResult> UpdateAsync(string id)
        {
            var dto = await _gameService.GetByIdAsync(id);
            var modifyGameViewModel = GetModifyGameViewModel(dto);
            var viewModel = await _gameViewModelFactory.CreateAsync(modifyGameViewModel);

            return View("Update", viewModel);
        }

        [HttpPost("update")]
        [IsAllowedUpdateGame]
        public async Task<IActionResult> UpdateAsync(ModifyGameViewModel modifyGameViewModel)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = await _gameViewModelFactory.CreateAsync(modifyGameViewModel);

                return View("Update", viewModel);
            }

            var dto = _mapper.Map<ModifyGameDto>(modifyGameViewModel);

            try
            {
                await _gameService.UpdateAsync(dto);
            }
            catch (ValidationException<GameRoot> e)
            {
                const string prefix = nameof(ModifyGameViewModel);
                ModelState.AddModelError($"{prefix}.{e.Key}", e.Message);

                var viewModel = await _gameViewModelFactory.CreateAsync(modifyGameViewModel);
                _logger.LogWarning(e.Message);

                return View("Update", viewModel);
            }

            _logger.LogDebug($"The game with id {modifyGameViewModel.Id} was updated in database");

            return RedirectToAction(nameof(GetAllAsync));
        }

        [HttpPost("remove")]
        [HasPermission(Permissions.DeleteGame)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _gameService.DeleteAsync(id);
            _logger.LogDebug($"The game with id {id} was deleted");

            return RedirectToAction(nameof(GetAllAsync));
        }

        [HttpGet("{key}/download")]
        public ActionResult<AppFile> DownloadFile(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return NotFound();
            }

            var file = _gameService.GetFile(key);
            _logger.LogDebug($"Getting file by game key {key}");

            return File(file.Data, file.Mime);
        }

        private ModifyGameViewModel GetModifyGameViewModel(ModifyGameDto modifyGameDto)
        {
            var defaultLocalization = modifyGameDto.Localizations.FirstOrDefault(dto => dto.CultureName == Culture.En);
            var russianLocalization = modifyGameDto.Localizations.FirstOrDefault(dto => dto.CultureName == Culture.Ru);

            var viewModel = _mapper.Map<ModifyGameViewModel>(modifyGameDto);
            _mapper.Map(defaultLocalization, viewModel);

            viewModel.GameLocalization = _mapper.Map<GameLocalizationViewModel>(russianLocalization);

            return viewModel;
        }

        private async Task<PageList<IEnumerable<GameDto>>> GetPageListAsync(
            FilterSelectedOptionsViewModel filterSelectedOptionsViewModel)
        {
            var filterData = _mapper.Map<GameFilterData>(filterSelectedOptionsViewModel);
            var pageList = await _gameService.GetPageListAsync(filterData, Culture.Current);

            return pageList;
        }

        private GamesCatalogueViewModel GetCatalogueGameViewModel(PageList<IEnumerable<GameDto>> pageList)
        {
            var displayGameViewModels = _mapper.Map<IEnumerable<DisplayGameViewModel>>(pageList.Model);
            var modelsWithNoImage = displayGameViewModels.Where(model => !model.Images.Any());
            
            foreach (var viewModel in modelsWithNoImage)
            {
                var defaultImage = _gameImageViewModelFactory.Create(viewModel.Key);
                viewModel.Images.Add(defaultImage);
            }
            
            var pageOptionsViewModel = _pageOptionsViewModelFactory.Create(pageList.PageOptions);

            var catalogue = new GamesCatalogueViewModel
            {
                Games = displayGameViewModels,
                PageOptions = pageOptionsViewModel,
            };

            return catalogue;
        }

        private async Task<FilterViewModel> GetFilterViewModelAsync(
            FilterSelectedOptionsViewModel selectedOptionsViewModel,
            PageList<IEnumerable<GameDto>> pageList)
        {
            var filterViewModel = await _filterViewModelFactory.CreateAsync(selectedOptionsViewModel);
            filterViewModel.FilterSelectionDataViewModel.MinPrice = pageList.MinPrice;
            filterViewModel.FilterSelectionDataViewModel.MaxPrice = pageList.MaxPrice;

            return filterViewModel;
        }
    }
}