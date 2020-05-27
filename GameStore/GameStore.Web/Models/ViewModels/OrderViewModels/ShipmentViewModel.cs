using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameStore.Web.Models.ViewModels.OrderViewModels
{
    public class ShipmentViewModel
    {
        public string OrderId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string ShipperEntityId { get; set; }
        public IEnumerable<SelectListItem> Shippers { get; set; }
    }
}