using System.Collections.Generic;
using GameStore.Web.Models.ViewModels.OrderViewModels;

namespace GameStore.Web.Models.ViewModels
{
    public class BasketViewModel
    {
        public string OrderId { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalCost { get; set; }
        public IEnumerable<OrderDetailsViewModel> OrderDetails { get; set; }
    }
}