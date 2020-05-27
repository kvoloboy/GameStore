using System;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Models;
using GameStore.Core.Models.Identity;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Filters;
using GameStore.Web.Models.ViewModels.GameViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameStore.Web.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class ApiGameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILogger<ApiGameController> _logger;
        private readonly IMapper _mapper;
        private readonly IAsyncViewModelFactory<GameDto, DisplayGameViewModel> _displayGameViewModelFactory;

        public ApiGameController(
            IGameService gameService,
            ILogger<ApiGameController> logger,
            IMapper mapper,
            IAsyncViewModelFactory<GameDto, DisplayGameViewModel> displayGameViewModelFactory)
        {
            _gameService = gameService;
            _logger = logger;
            _mapper = mapper;
            _displayGameViewModelFactory = displayGameViewModelFactory;
        }
        
        [HttpPost("new")]
        [ApiAuthorizationFilter(Permissions.CreateGame)]
        public async Task<IActionResult> CreateAsync(ModifyGameViewModel modifyGameViewModel)
        {
            modifyGameViewModel.Id = Guid.NewGuid().ToString();
            var dto = _mapper.Map<ModifyGameDto>(modifyGameViewModel);

            try
            {
                await _gameService.CreateAsync(dto);
            }
            catch (ValidationException<GameRoot> e)
            {
                const string prefix = nameof(ModifyGameViewModel);
                ModelState.AddModelError($"{prefix}.{e.Key}", e.Message);

                _logger.LogWarning(e.Message);

                return BadRequest(ModelState);
            }

            _logger.LogDebug($"A new game with name: {modifyGameViewModel.Name} was added to database");

            return CreatedAtAction(nameof(GetByIdAsync), new{id = modifyGameViewModel.Id}, modifyGameViewModel);
        }

        [HttpPut("{id}")]
        [ApiAuthorizationFilter(Permissions.UpdateGame)]
        public async Task<IActionResult> UpdateAsync(ModifyGameViewModel modifyGameViewModel)
        {
            var dto = _mapper.Map<ModifyGameDto>(modifyGameViewModel);

            try
            {
                await _gameService.UpdateAsync(dto);
            }
            catch (ValidationException<GameRoot> e)
            {
                const string prefix = nameof(ModifyGameViewModel);
                ModelState.AddModelError($"{prefix}.{e.Key}", e.Message);

                _logger.LogWarning(e.Message);

                return BadRequest(ModelState);
            }

            _logger.LogDebug($"The game with id {modifyGameViewModel.Id} was updated in database");

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ApiAuthorizationFilter(Permissions.DeleteGame)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _gameService.DeleteAsync(id);
            _logger.LogDebug($"The comment with id {id} was deleted");

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DisplayGameViewModel>> GetByIdAsync(string id)
        {
            var gameDto = await _gameService.GetByIdAsync(id, Culture.Current);
            var viewModel = await _displayGameViewModelFactory.CreateAsync(gameDto);

            return viewModel;
        }
    }
}