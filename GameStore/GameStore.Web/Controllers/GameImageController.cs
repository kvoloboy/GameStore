using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Web.Models.ViewModels.ImageViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameStore.Web.Controllers
{
    [Route("images")]
    public class GameImageController : Controller
    {
        private readonly IGameImageService _gameImageService;
        private readonly IMapper _mapper;
        private readonly ILogger<GameImageController> _logger;

        public GameImageController(
            IGameImageService gameImageService,
            IMapper mapper,
            ILogger<GameImageController> logger)
        {
            _gameImageService = gameImageService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> IndexAsync(string key)
        {
            var images = await _gameImageService.GetByGameKeyAsync(key);
            var imageViewModels = _mapper.Map<IEnumerable<GameImageViewModel>>(images);
            var viewModel = new ImageIndexViewModel
            {
                GameKey = key,
                Images = imageViewModels
            };

            return View("Index", viewModel);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync(ModifyImageViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(IndexAsync), new {key = viewModel.GameKey});
            }

            var gameImageDto = await GetImageDtoAsync(viewModel);

            await _gameImageService.CreateAsync(gameImageDto);
            _logger.LogDebug($"Add image to game with key: {viewModel.GameKey}");

            return RedirectToAction(nameof(IndexAsync), new {key = viewModel.GameKey});
        }
        
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _gameImageService.DeleteAsync(id);
            _logger.LogDebug($"Delete game image with id: {id}");

            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet("{id}/sync")]
        public IActionResult GetById(string id)
        {
            var imageTask = _gameImageService.GetByIdAsync(id).ConfigureAwait(false);
            var image = imageTask.GetAwaiter().GetResult();

            return File(image.Content, image.ContentType);
        }

        [HttpGet("{id}/async")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var image = await _gameImageService.GetByIdAsync(id);

            return File(image.Content, image.ContentType);
        }

        private static async Task<GameImageDto> GetImageDtoAsync(ModifyImageViewModel viewModel)
        {
            await using var stream = new MemoryStream();
            await viewModel.Image.CopyToAsync(stream);

            var gameImageDto = new GameImageDto
            {
                Id = viewModel.Id,
                Content = stream.ToArray(),
                GameKey = viewModel.GameKey,
                ContentType = viewModel.Image.ContentType
            };

            return gameImageDto;
        }
    }
}