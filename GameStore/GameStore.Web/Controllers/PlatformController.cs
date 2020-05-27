using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models;
using GameStore.Core.Models.Identity;
using GameStore.Identity.Attributes;
using GameStore.Web.Models.ViewModels.PlatformViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameStore.Web.Controllers
{
    [Route("platform")]
    public class PlatformController : Controller
    {
        private readonly IPlatformService _platformServices;
        private readonly ILogger<PlatformController> _logger;
        private readonly IMapper _mapper;

        public PlatformController(
            IPlatformService platformServices,
            ILogger<PlatformController> logger,
            IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
            _platformServices = platformServices;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var platforms = await _platformServices.GetAllAsync();
            var platformsViewModel = _mapper.Map<IEnumerable<PlatformViewModel>>(platforms);

            return View("Index", platformsViewModel);
        }


        [HttpGet("new")]
        [HasPermission(Permissions.CreatePlatform)]
        public IActionResult Create()
        {
            return View(new PlatformViewModel());
        }

        [HttpPost("new")]
        [HasPermission(Permissions.CreatePlatform)]
        public async Task<IActionResult> CreateAsync(PlatformViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", viewModel);
            }

            var dto = _mapper.Map<PlatformDto>(viewModel);

            try
            {
                await _platformServices.CreateAsync(dto);
            }
            catch (ValidationException<Platform> e)
            {
                ModelState.AddModelError(e.Key, e.Message);
                _logger.LogWarning(e.Message);

                return View("Create", viewModel);
            }

            _logger.LogDebug($"Create new platform with name {viewModel.Name}");

            return RedirectToAction(nameof(GetAllAsync));
        }

        [HttpGet("update")]
        [HasPermission(Permissions.UpdatePlatform)]
        public async Task<IActionResult> UpdateAsync(string id)
        {
            var genreDto = await _platformServices.GetByIdAsync(id);
            var viewModel = _mapper.Map<PlatformViewModel>(genreDto);

            return View("Update", viewModel);
        }

        [HttpPost("update")]
        [HasPermission(Permissions.UpdatePlatform)]
        public async Task<IActionResult> UpdateAsync(PlatformViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Update", viewModel);
            }

            var dto = _mapper.Map<PlatformDto>(viewModel);

            try
            {
                await _platformServices.UpdateAsync(dto);
            }
            catch (ValidationException<Platform> e)
            {
                ModelState.AddModelError(e.Key, e.Message);
                _logger.LogWarning(e.Message);

                return View("Update", viewModel);
            }

            _logger.LogDebug($"Update platform with id {viewModel.Id}");

            return RedirectToAction(nameof(GetAllAsync));
        }

        [HttpPost("delete/{id}")]
        [HasPermission(Permissions.DeletePlatform)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _platformServices.DeleteAsync(id);
            _logger.LogDebug($"Delete platform with id {id}");

            return RedirectToAction(nameof(GetAllAsync));
        }
    }
}