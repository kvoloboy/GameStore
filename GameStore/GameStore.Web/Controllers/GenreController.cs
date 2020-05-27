using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models;
using GameStore.Core.Models.Identity;
using GameStore.Identity.Attributes;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.GenreViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameStore.Web.Controllers
{
    [Route("genre")]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreServices;
        private readonly IAsyncViewModelFactory<ModifyGenreViewModel, GenreViewModel> _genreViewModelFactory;
        private readonly ILogger<GenreController> _logger;
        private readonly IMapper _mapper;

        public GenreController(
            IGenreService genreServices,
            IAsyncViewModelFactory<ModifyGenreViewModel, GenreViewModel> genreViewModelFactory,
            ILogger<GenreController> logger,
            IMapper mapper)
        {
            _genreServices = genreServices;
            _genreViewModelFactory = genreViewModelFactory;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _genreServices.GetAllAsync();
            var genresViewModel = _mapper.Map<IEnumerable<ModifyGenreViewModel>>(genres);

            return View("Index", genresViewModel);
        }

        [HttpGet("new")]
        [HasPermission(Permissions.CreateGenre)]
        public async Task<IActionResult> CreateAsync()
        {
            var viewModel = await _genreViewModelFactory.CreateAsync(new ModifyGenreViewModel());

            return View("Create", viewModel);
        }

        [HttpPost("new")]
        [HasPermission(Permissions.CreateGenre)]
        public async Task<IActionResult> CreateAsync(
            [Bind(Prefix = nameof(ModifyGenreViewModel))]
            ModifyGenreViewModel modifyGenreViewModel)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = await _genreViewModelFactory.CreateAsync(modifyGenreViewModel);

                return View("Create", viewModel);
            }

            var dto = _mapper.Map<GenreDto>(modifyGenreViewModel);

            try
            {
                await _genreServices.CreateAsync(dto);
            }
            catch (EntityExistsWithKeyValueException<Genre> e)
            {
                const string prefix = nameof(ModifyGenreViewModel);
                ModelState.AddModelError($"{prefix}.{e.Key}", e.Message);

                var viewModel = await _genreViewModelFactory.CreateAsync(modifyGenreViewModel);
                _logger.LogWarning(e.Message);

                return View("Create", viewModel);
            }

            _logger.LogDebug($"Create new genre with name {modifyGenreViewModel.Name}");

            return RedirectToAction(nameof(GetAllAsync));
        }

        [HttpGet("update")]
        [HasPermission(Permissions.UpdateGenre)]
        public async Task<IActionResult> UpdateAsync(string id)
        {
            var genreDto = await _genreServices.GetByIdAsync(id);
            var modifyGenreViewModel = _mapper.Map<ModifyGenreViewModel>(genreDto);
            var genreViewModel = await _genreViewModelFactory.CreateAsync(modifyGenreViewModel);

            return View("Update", genreViewModel);
        }

        [HttpPost("update")]
        [HasPermission(Permissions.UpdateGenre)]
        public async Task<IActionResult> UpdateAsync(
            [Bind(Prefix = nameof(ModifyGenreViewModel))]
            ModifyGenreViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var genreViewModel = await _genreViewModelFactory.CreateAsync(viewModel);

                return View("Update", genreViewModel);
            }

            var dto = _mapper.Map<GenreDto>(viewModel);

            try
            {
                await _genreServices.UpdateAsync(dto);
            }
            catch (ValidationException<Genre> e)
            {
                const string prefix = nameof(ModifyGenreViewModel);
                var errorKey = string.IsNullOrEmpty(e.Key) ? e.Key : $"{prefix}.{e.Key}";
                ModelState.AddModelError(errorKey, e.Message);

                var genreViewModel = await _genreViewModelFactory.CreateAsync(viewModel);
                _logger.LogWarning(e.Message);

                return View("Update", genreViewModel);
            }

            _logger.LogDebug($"Update genre with id {viewModel.Id}");

            return RedirectToAction(nameof(GetAllAsync));
        }

        [HttpPost("delete/{id}")]
        [HasPermission(Permissions.DeleteGenre)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _genreServices.DeleteAsync(id);
            _logger.LogDebug($"Delete genre with id {id}");

            return RedirectToAction(nameof(GetAllAsync));
        }
    }
}