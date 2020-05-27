using System.Collections.Generic;
using GameStore.BusinessLayer.DTO;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Models
{
    public class Basket
    {
        public string OrderId { get; set; }
        public decimal TotalItems { get; set; }
        public decimal TotalCost { get; set; }
        public IEnumerable<OrderDetailsDto> OrderDetails { get; set; }
    }
}