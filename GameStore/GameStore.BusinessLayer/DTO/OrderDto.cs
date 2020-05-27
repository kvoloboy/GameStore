using System;
using System.Collections.Generic;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.DTO
{
    public class OrderDto
    {
        public string Id { get; set; }
        public ShipmentDto Shipment { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public OrderState State { get; set; }
        public ICollection<OrderDetailsDto> Details { get; set; } = new List<OrderDetailsDto>();
        public Shipper Shipper { get; set; }
    }
}