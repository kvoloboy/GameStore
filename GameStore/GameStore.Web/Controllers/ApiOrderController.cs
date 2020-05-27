using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Models.Identity;
using GameStore.Identity.Extensions;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Filters;
using GameStore.Web.Models.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameStore.Web.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class ApiOrderController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        private readonly IAsyncViewModelFactory<string, DetailedOrderViewModel> _detailedOrderViewModelFactory;
        private readonly IGameService _gameService;
        private readonly ILogger<ApiOrderController> _logger;
        private readonly IMapper _mapper;

        public ApiOrderController(
            IBasketService basketService,
            IOrderService orderService,
            IAsyncViewModelFactory<string, DetailedOrderViewModel> detailedOrderViewModelFactory,
            IGameService gameService,
            ILogger<ApiOrderController> logger,
            IMapper mapper)
        {
            _basketService = basketService;
            _orderService = orderService;
            _detailedOrderViewModelFactory = detailedOrderViewModelFactory;
            _gameService = gameService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetailedOrderViewModel>> GetByIdAsync(string id)
        {
            var order = await _detailedOrderViewModelFactory.CreateAsync(id);

            return order;
        }

        [HttpPost("{game-id}")]
        public async Task<IActionResult> CreateAsync(string gameId)
        {
            var game = await _gameService.GetByIdAsync(gameId, Culture.Current);

            var orderId = await _basketService.AddAsync(game.Key, User?.GetId());
            var order = await _detailedOrderViewModelFactory.CreateAsync(orderId);
            _logger.LogDebug($"Create order with id {orderId}");
            
            return CreatedAtAction(nameof(GetByIdAsync), new {id = orderId}, order);
        }
        
        [HttpPut("{id}")]
        [ApiAuthorizationFilter(Permissions.UpdateOrder)]
        public async Task<IActionResult> UpdateAsync(ModifyOrderViewModel viewModel)
        {
            var orderDto = _mapper.Map<OrderDto>(viewModel);
            await _orderService.UpdateAsync(orderDto);
            _logger.LogWarning($"Update order with id {viewModel.Id}");
           
            return NoContent();
        }
    }
}