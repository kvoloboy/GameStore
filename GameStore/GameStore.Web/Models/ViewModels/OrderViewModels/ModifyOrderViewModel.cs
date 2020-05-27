using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Core.Models;

namespace GameStore.Web.Models.ViewModels.OrderViewModels
{
    public class ModifyOrderViewModel
    {
        [Required]
        public string Id { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        public ShipmentViewModel Shipment { get; set; }
        public IEnumerable<ModifyOrderDetailsViewModel> Details { get; set; }
        public OrderState State { get; set; }
    }
}