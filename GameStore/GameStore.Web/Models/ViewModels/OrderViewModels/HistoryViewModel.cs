using System;
using System.Collections.Generic;

namespace GameStore.Web.Models.ViewModels.OrderViewModels
{
    public class OrdersListViewModel
    {
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public IEnumerable<DisplayOrderViewModel> DisplayOrderViewModels { get; set; }
    }
}