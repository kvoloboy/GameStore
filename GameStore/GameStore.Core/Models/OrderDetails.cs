using System;

namespace GameStore.Core.Models
{
    public class OrderDetails
    {
        public OrderDetails()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public string Id { get; set; }
        public short Quantity { get; set; } = 1;
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string OrderId { get; set; }
        public Order Order { get; set; }
        public string GameRootId { get; set; }
        public GameRoot GameRoot { get; set; }
    }
}