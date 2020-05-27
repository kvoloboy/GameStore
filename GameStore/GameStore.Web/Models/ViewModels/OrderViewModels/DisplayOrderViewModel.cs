using System;
using System.Collections.Generic;

namespace GameStore.Web.Models.ViewModels.OrderViewModels
{
    public class DisplayOrderViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ShipperName { get; set; }
        public decimal Total { get; set; }
        public int ProductsQuantity { get; set; }
        public DateTime OrderDate { get; set; }
        public KeyValuePair<string, string> State { get; set; }
        public bool CanBeChanged { get; set; }
    }
}