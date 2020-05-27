using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models;
using GameStore.Web.WebServices.Payments.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.WebServices.Payments
{
    public class BankPaymentStrategy : IPaymentStrategy
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IOrderService _orderService;

        public BankPaymentStrategy(IInvoiceService invoiceService, IOrderService orderService)
        {
            _invoiceService = invoiceService;
            _orderService = orderService;
        }

        public IActionResult Pay(OrderDto orderDto)
        {
            SetPendingState(orderDto.Id);
            var total = _orderService.ComputeTotal(orderDto.Details);
            var invoiceFile = _invoiceService.CreateInvoiceFile(orderDto.Id, orderDto.UserId, total);
            
            return new FileContentResult(invoiceFile.Data, invoiceFile.Mime)
            {
                FileDownloadName = invoiceFile.Name
            };
        }

        private void SetPendingState(string orderId)
        {
            _orderService.SetStateAsync(orderId, OrderState.Pending);
        }
    }
}