using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Models.Identity;
using GameStore.Web.Filters;
using GameStore.Web.Models.ViewModels.GameViewModels;
using GameStore.Web.Models.ViewModels.PublisherViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameStore.Web.Controllers
{
    [ApiController]
    [Route("api/publishers")]
    public class ApiPublisherController : ControllerBase
    {
        private readonly IPublisherService _publisherServices;
        private readonly IGameService _gameService;
        private readonly ILogger<ApiPublisherController> _logger;
        private readonly IMapper _mapper;

        public ApiPublisherController(
            IPublisherService publisherServices,
            IGameService gameService,
            ILogger<ApiPublisherController> logger,
            IMapper mapper)
        {
            _publisherServices = publisherServices;
            _gameService = gameService;
            _logger = logger;
            _mapper = mapper;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherViewModel>> GetByIdAsync(string id)
        {
            var publisher = await _publisherServices.GetByIdAsync(id, Culture.Current);
            var viewModel = _mapper.Map<PublisherViewModel>(publisher);

            return viewModel;
        }

        [HttpPost("new")]
        [ApiAuthorizationFilter(Permissions.CreatePublisher)]
        public async Task<IActionResult> CreateAsync(ModifyPublisherViewModel viewModel)
        {
            viewModel.Id = Guid.NewGuid().ToString();

            var dto = _mapper.Map<ModifyPublisherDto>(viewModel);
            await _publisherServices.CreateAsync(dto);
            _logger.LogDebug($"Create new publisher with name {viewModel.CompanyName}");

            return CreatedAtAction(nameof(GetByIdAsync), new {id = viewModel.Id}, viewModel);
        }

        [HttpPut("{id}")]
        [ApiAuthorizationFilter(Permissions.UpdatePublisher)]
        public async Task<IActionResult> UpdateAsync(ModifyPublisherViewModel publisherViewModel)
        {
            var dto = _mapper.Map<ModifyPublisherDto>(publisherViewModel);
            await _publisherServices.UpdateAsync(dto);
            _logger.LogDebug($"Update publisher with id {publisherViewModel.Id}");

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ApiAuthorizationFilter(Permissions.DeletePublisher)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _publisherServices.DeleteAsync(id);
            _logger.LogDebug($"Delete publisher with id {id}");

            return NoContent();
        }

        [HttpGet("{id}/games")]
        public async Task<ActionResult<IEnumerable<DisplayGameViewModel>>> GetGamesByPublisherAsync(string id)
        {
            var games = await _gameService.GetByPublisherAsync(id);
            var viewModels = _mapper.Map<IEnumerable<DisplayGameViewModel>>(games).ToList();

            return viewModels;
        }
    }
}