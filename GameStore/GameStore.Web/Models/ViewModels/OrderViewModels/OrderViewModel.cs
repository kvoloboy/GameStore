using System.Collections.Generic;
using GameStore.Web.Models.ViewModels.PaymentViewModels;

namespace GameStore.Web.Models.ViewModels.OrderViewModels
{
    public class OrderViewModel
    {
        public BasketViewModel Basket { get; set; }
        public IEnumerable<PaymentTypeViewModel> Payments { get; set; }
    }
}