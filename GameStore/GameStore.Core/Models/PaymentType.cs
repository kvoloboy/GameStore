using System;

namespace GameStore.Core.Models
{
    public class PaymentType
    {
        public PaymentType()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}