using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameStore.Web.Models.ViewModels.OrderViewModels
{
    public class ModifyOrderDetailsViewModel
    {
        public string Id { get; set; }
        
        public short Quantity { get; set; }
        
        [Required]
        public string OrderId { get; set; }
        
        [Required]
        public string GameId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        
        [Range(0, 100)]
        public decimal Discount { get; set; }
        
        public IEnumerable<SelectListItem> Games { get; set; }
    }
}