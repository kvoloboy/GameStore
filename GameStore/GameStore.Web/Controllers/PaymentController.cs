using System.Threading.Tasks;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models.Identity;
using GameStore.Identity.Attributes;
using GameStore.Identity.Extensions;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models;
using GameStore.Web.Models.ViewModels.PaymentViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers
{
    [Route("order/checkout")]
    public class PaymentController : Controller
    {
        private readonly IPaymentStrategyFactory _paymentStrategyFactory;
        private readonly IOrderService _orderService;

        public PaymentController(IPaymentStrategyFactory paymentStrategyFactory,
            IOrderService orderService)
        {
            _paymentStrategyFactory = paymentStrategyFactory;
            _orderService = orderService;
        }

        [HttpGet]
        [HasPermission(Permissions.MakeOrder)]
        public async Task<IActionResult> CheckoutAsync(string payment)
        {
            if (string.IsNullOrEmpty(payment))
            {
                return BadRequest();
            }

            var userId = User?.GetId();
            var order = await _orderService.GetNewOrderByUserIdAsync(userId);
            var strategy = _paymentStrategyFactory.Create(payment);

            return strategy.Pay(order);
        }

        [HttpPost]
        [HasPermission(Permissions.MakeOrder)]
        public IActionResult VisaCheckout(VisaViewModel visaViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("VisaPayment", visaViewModel);
            }

            _orderService.ConfirmAsync(visaViewModel.OrderId);

            return RedirectToAction("Index", "Home");
        }
    }
}