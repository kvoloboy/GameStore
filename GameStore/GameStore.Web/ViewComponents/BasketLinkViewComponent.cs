using System.Linq;
using System.Threading.Tasks;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Identity.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.ViewComponents
{
    public class BasketLinkViewComponent : ViewComponent
    {
        private readonly IBasketService _basketService;

        public BasketLinkViewComponent(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = UserClaimsPrincipal?.GetId();
            var detailsCount = 0;

            if (string.IsNullOrEmpty(userId))
            {
                return View(detailsCount);
            }

            var basket = await _basketService.GetBasketForUserAsync(userId);
            detailsCount = basket.OrderDetails.Sum(d => d.Quantity);

            return View(detailsCount);
        }
    }
}