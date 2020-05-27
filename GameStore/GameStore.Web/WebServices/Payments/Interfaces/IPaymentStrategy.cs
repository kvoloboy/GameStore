using GameStore.BusinessLayer.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.WebServices.Payments.Interfaces
{
    public interface IPaymentStrategy
    {
        IActionResult Pay(OrderDto orderDto);
    }
}