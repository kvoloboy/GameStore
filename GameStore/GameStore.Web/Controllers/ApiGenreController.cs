using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models;
using GameStore.Core.Models.Identity;
using GameStore.Web.Filters;
using GameStore.Web.Models.ViewModels.GameViewModels;
using GameStore.Web.Models.ViewModels.GenreViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameStore.Web.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class ApiGenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;
        private readonly ILogger<ApiGenreController> _logger;

        public ApiGenreController(
            IGenreService genreService,
            IGameService gameService,
            IMapper mapper,
            ILogger<ApiGenreController> logger)
        {
            _genreService = genreService;
            _gameService = gameService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ModifyGenreViewModel>> GetByIdAsync(string id)
        {
            var genre = await _genreService.GetByIdAsync(id);
            var viewModel = _mapper.Map<ModifyGenreViewModel>(genre);

            return viewModel;
        }

        [HttpGet("{id}/games")]
        public async Task<ActionResult<IEnumerable<DisplayGameViewModel>>> GetGamesByGenreAsync(string id)
        {
            var gamesDto = await _gameService.GetByGenreAsync(id);
            var displayGameViewModels = _mapper.Map<IEnumerable<DisplayGameViewModel>>(gamesDto).ToList();

            return displayGameViewModels;
        }

        [HttpPost("new")]
        [ApiAuthorizationFilter(Permissions.CreateGenre)]
        public async Task<IActionResult> CreateAsync(ModifyGenreViewModel modifyGenreViewModel)
        {
            modifyGenreViewModel.Id = Guid.NewGuid().ToString();
            var dto = _mapper.Map<GenreDto>(modifyGenreViewModel);

            try
            {
                await _genreService.CreateAsync(dto);
            }
            catch (ValidationException<Genre> e)
            {
                const string prefix = nameof(ModifyGenreViewModel);
                ModelState.AddModelError($"{prefix}.{e.Key}", e.Message);

                _logger.LogWarning(e.Message);

                return BadRequest(ModelState);
            }

            _logger.LogDebug($"Create new genre with name {modifyGenreViewModel.Name}");

            return CreatedAtAction(nameof(GetByIdAsync), new {id = modifyGenreViewModel.Id}, modifyGenreViewModel);
        }

        [HttpPut("{id}")]
        [ApiAuthorizationFilter(Permissions.UpdateGenre)]
        public async Task<IActionResult> UpdateAsync(ModifyGenreViewModel viewModel)
        {
            var dto = _mapper.Map<GenreDto>(viewModel);

            try
            {
                await _genreService.UpdateAsync(dto);
            }
            catch (ValidationException<Genre> e)
            {
                const string prefix = nameof(ModifyGenreViewModel);
                var errorKey = string.IsNullOrEmpty(e.Key) ? e.Key : $"{prefix}.{e.Key}";
                ModelState.AddModelError(errorKey, e.Message);

                _logger.LogWarning(e.Message);

                return BadRequest(ModelState);
            }

            _logger.LogDebug($"Update genre with id {viewModel.Id}");

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ApiAuthorizationFilter(Permissions.DeleteGenre)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _genreService.DeleteAsync(id);
            _logger.LogDebug($"Delete genre with id {id}");

            return NoContent();
        }
    }
}