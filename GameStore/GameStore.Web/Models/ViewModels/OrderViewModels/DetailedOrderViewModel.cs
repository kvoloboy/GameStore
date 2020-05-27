using System.Collections.Generic;

namespace GameStore.Web.Models.ViewModels.OrderViewModels
{
    public class DetailedOrderViewModel
    {
        public string OrderId { get; set; }
        public string ShipperName { get; set; }
        public string TotalSum { get; set; }
        public string TotalItems { get; set; }
        public ShipmentViewModel ShipmentViewModel { get; set; }
        public IEnumerable<OrderDetailsViewModel> OrderDetailsViewModels { get; set; }
        public bool CanBeUpdated { get; set; }
        public string State { get; set; }
    }
}