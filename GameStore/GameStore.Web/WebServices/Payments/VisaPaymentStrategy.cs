using GameStore.BusinessLayer.DTO;
using GameStore.Web.Models.ViewModels.PaymentViewModels;
using GameStore.Web.WebServices.Payments.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GameStore.Web.WebServices.Payments
{
    public class VisaPaymentStrategy : IPaymentStrategy
    {
        public IActionResult Pay(OrderDto orderDto)
        {
            const string viewName = "VisaPayment";
            var viewModel = new VisaViewModel {OrderId = orderDto.Id};
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = viewModel
            };

            return new ViewResult
            {
                ViewName = viewName,
                ViewData = viewData
            };
        }
    }
}