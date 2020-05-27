using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models.Identity;
using GameStore.Identity.Attributes;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers
{
    [Route("order-details")]
    public class OrderDetailsController : Controller
    {
        private readonly IOrderDetailsService _orderDetailsService;
        private readonly IAsyncViewModelFactory<ModifyOrderDetailsViewModel, ModifyOrderDetailsViewModel>
            _modifyOrderDetailsViewModelFactory;
        private readonly IMapper _mapper;

        public OrderDetailsController(IOrderDetailsService orderDetailsService,
            IAsyncViewModelFactory<ModifyOrderDetailsViewModel, ModifyOrderDetailsViewModel> orderDetailsViewModelFactory,
            IMapper mapper)
        {
            _orderDetailsService = orderDetailsService;
            _modifyOrderDetailsViewModelFactory = orderDetailsViewModelFactory;
            _mapper = mapper;
        }

        [HttpGet("create")]
        [HasPermission(Permissions.UpdateOrder)]
        public async Task<IActionResult> CreateAsync(string orderId)
        {
            var viewModel = new ModifyOrderDetailsViewModel {OrderId = orderId};
            var filledViewModel = await _modifyOrderDetailsViewModelFactory.CreateAsync(viewModel);

            return View("Create", filledViewModel);
        }

        [HttpPost("create")]
        [HasPermission(Permissions.UpdateOrder)]
        public async Task<IActionResult> CreateAsync(ModifyOrderDetailsViewModel modifyOrderDetailsViewModel)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = await _modifyOrderDetailsViewModelFactory.CreateAsync(modifyOrderDetailsViewModel);

                return View("Create", viewModel);
            }

            var orderDetailsDto = _mapper.Map<OrderDetailsDto>(modifyOrderDetailsViewModel);
            await _orderDetailsService.CreateAsync(orderDetailsDto);

            return RedirectToAction("ListAsync", "Order");
        }

        [HttpGet("update")]
        [HasPermission(Permissions.UpdateOrder)]
        public async Task<IActionResult> UpdateAsync(string id)
        {
            var detailsDto = await _orderDetailsService.GetByIdAsync(id);
            var viewModel = _mapper.Map<ModifyOrderDetailsViewModel>(detailsDto);
            var filledViewModel = await _modifyOrderDetailsViewModelFactory.CreateAsync(viewModel);

            return View("Update", filledViewModel);
        }

        [HttpPost("update")]
        [HasPermission(Permissions.UpdateOrder)]
        public async Task<IActionResult> UpdateAsync(ModifyOrderDetailsViewModel modifyOrderDetailsViewModel)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = await _modifyOrderDetailsViewModelFactory.CreateAsync(modifyOrderDetailsViewModel);

                return View("Update", viewModel);
            }

            var dto = _mapper.Map<OrderDetailsDto>(modifyOrderDetailsViewModel);
            await _orderDetailsService.UpdateAsync(dto);

            return RedirectToAction("ListAsync", "Order");
        }

        [HttpPost("delete")]
        [HasPermission(Permissions.UpdateOrder)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _orderDetailsService.DeleteAsync(id);

            return RedirectToAction("ListAsync", "Order");
        }
    }
}