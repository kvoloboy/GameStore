using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.Core.Models.Identity;

namespace GameStore.Core.Models
{
    public class Order
    {
        public Order()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public string ShipperEntityId { get; set; }
        public Shipper Shipper { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public double Freight { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public OrderState State { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<OrderDetails> Details { get; set; }
    }
}