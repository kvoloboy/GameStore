using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Models.Identity;
using GameStore.Identity.Attributes;
using GameStore.Web.Filters;
using GameStore.Web.Models.ViewModels.PublisherViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameStore.Web.Controllers
{
    [Route("publisher")]
    public class PublisherController : Controller
    {
        private readonly IPublisherService _publisherServices;
        private readonly ILogger<PublisherController> _logger;
        private readonly IMapper _mapper;

        public PublisherController(
            IPublisherService publisherServices,
            ILogger<PublisherController> logger,
            IMapper mapper)
        {
            _publisherServices = publisherServices;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetAllAsync()
        {
            var publishersDto = await _publisherServices.GetAllAsync(Culture.Current);
            var publishersViewModel = _mapper.Map<IEnumerable<PublisherViewModel>>(publishersDto);

            return View("Index", publishersViewModel);
        }

        [HttpGet("{company-name}")]
        public async Task<IActionResult> DetailsAsync(string companyName)
        {
            if (string.IsNullOrEmpty(companyName))
            {
                return NotFound();
            }

            var publisher = await _publisherServices.GetByCompanyAsync(companyName, Culture.Current);
            var viewModel = _mapper.Map<PublisherViewModel>(publisher);

            return View("Details", viewModel);
        }

        [HttpGet("new")]
        [HasPermission(Permissions.CreatePublisher)]
        public IActionResult Create()
        {
            return View(new ModifyPublisherViewModel());
        }

        [HttpPost("new")]
        [HasPermission(Permissions.CreatePublisher)]
        public async Task<IActionResult> CreateAsync(ModifyPublisherViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", viewModel);
            }

            var dto = _mapper.Map<ModifyPublisherDto>(viewModel);
            await _publisherServices.CreateAsync(dto);
            _logger.LogDebug($"Create new publisher with name {viewModel.CompanyName}");

            return RedirectToAction(nameof(GetAllAsync));
        }

        [HttpGet("update")]
        [IsAllowedUpdatePublisher]
        public async Task<IActionResult> UpdateAsync(string id)
        {
            var modifyPublisherDto = await _publisherServices.GetByIdAsync(id);
            var viewModel = GetModifyPublisherViewModel(modifyPublisherDto);

            return View("Update", viewModel);
        }

        [HttpPost("update")]
        [IsAllowedUpdatePublisher]
        public async Task<IActionResult> UpdateAsync(ModifyPublisherViewModel publisherViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Update", publisherViewModel);
            }

            var dto = _mapper.Map<ModifyPublisherDto>(publisherViewModel);
            await _publisherServices.UpdateAsync(dto);
            _logger.LogDebug($"Update publisher with id {publisherViewModel.Id}");

            return RedirectToAction(nameof(GetAllAsync));
        }

        [HttpPost("delete/{id}")]
        [HasPermission(Permissions.DeletePublisher)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _publisherServices.DeleteAsync(id);
            _logger.LogDebug($"Delete publisher with id {id}");

            return RedirectToAction(nameof(GetAllAsync));
        }
        
        private ModifyPublisherViewModel GetModifyPublisherViewModel(ModifyPublisherDto publisherDto)
        {
            var defaultLocalization = publisherDto.Localizations.FirstOrDefault(dto => dto.CultureName == Culture.En);
            var russianLocalization = publisherDto.Localizations.FirstOrDefault(dto => dto.CultureName == Culture.Ru);
            var viewModel = _mapper.Map<ModifyPublisherViewModel>(publisherDto);
            _mapper.Map(defaultLocalization, viewModel);
            viewModel.PublisherLocalization = _mapper.Map<PublisherLocalizationViewModel>(russianLocalization);

            return viewModel;
        }
    }
}