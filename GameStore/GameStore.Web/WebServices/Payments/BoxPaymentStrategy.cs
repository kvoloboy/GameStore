using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models;
using GameStore.Web.Models.ViewModels.PaymentViewModels;
using GameStore.Web.WebServices.Payments.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GameStore.Web.WebServices.Payments
{
    public class BoxPaymentStrategy : IPaymentStrategy
    {
        private readonly IOrderService _orderService;

        public BoxPaymentStrategy(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Pay(OrderDto orderDto)
        {
            const string viewName = "BoxPayment";
            var paymentDetails = CreateDetails(orderDto);
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = paymentDetails
            };

            return new ViewResult
            {
                ViewName = viewName,
                ViewData = viewData
            };
        }

        private PaymentDetailsViewModel CreateDetails(OrderDto orderDto)
        {
            var total = _orderService.ComputeTotal(orderDto.Details);
            var viewModel = new PaymentDetailsViewModel
            {
                UserId = orderDto.UserId,
                OrderId = orderDto.Id,
                Total = total
            };

            return viewModel;
        }
    }
}