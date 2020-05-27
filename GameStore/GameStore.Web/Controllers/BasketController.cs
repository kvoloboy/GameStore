using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Models;
using GameStore.Identity.Extensions;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels;
using GameStore.Web.Models.ViewModels.ImageViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameStore.Web.Controllers
{
    [Route("basket")]
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IGameService _gameService;
        private readonly ILogger<BasketController> _logger;
        private readonly IMapper _mapper;

        public BasketController(
            IBasketService basketService,
            ILogger<BasketController> logger,
            IMapper mapper,
            IGameService gameService)
        {
            _basketService = basketService;
            _gameService = gameService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> IndexAsync()
        {
            var basketViewModel = await GetBasketViewModelAsync();
            
            foreach (var orderDetails in basketViewModel.OrderDetails)
            {
                orderDetails.Price = _gameService.ComputePriceWithDiscount(orderDetails.Price, orderDetails.Discount);
            }
            
            return View("Index", basketViewModel);
        }

        [HttpPost("game/{key}/buy")]
        public async Task<IActionResult> AddAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return BadRequest();
            }

            var userId = User?.GetId();
            await _basketService.AddAsync(key, userId);

            return RedirectToAction(nameof(IndexAsync));
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateQuantityAsync(string detailsId, short quantity)
        {
            try
            {
                await _basketService.UpdateQuantityAsync(detailsId, quantity);
            }
            catch (ValidationException<OrderDetails> e)
            {
                ModelState.AddModelError(e.Key, e.Message);
                _logger.LogWarning(e.Message);

                return BadRequest(ModelState);
            }
            
            var basketViewModel = await GetBasketViewModelAsync();
            return PartialView("Total", basketViewModel.TotalCost);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _basketService.DeleteAsync(id);

            var basketViewModel = await GetBasketViewModelAsync();
            
            return PartialView("Total", basketViewModel.TotalCost);
        }

        private async Task<BasketViewModel> GetBasketViewModelAsync()
        {
            var userId = User?.GetId();
            var basket = await _basketService.GetBasketForUserAsync(userId, Culture.Current);
            var basketViewModel = _mapper.Map<BasketViewModel>(basket);
            
            return basketViewModel;
        }
    }
}