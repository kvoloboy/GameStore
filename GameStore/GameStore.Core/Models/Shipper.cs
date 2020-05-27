using System;

namespace GameStore.Core.Models
{
    public class Shipper
    {
        public Shipper()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public string Id { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
    }
}