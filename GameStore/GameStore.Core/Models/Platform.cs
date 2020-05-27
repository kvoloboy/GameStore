using System;
using System.Collections.Generic;

namespace GameStore.Core.Models
{
    public class Platform
    {
        public Platform()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<GamePlatform> GamePlatforms { get; set; }
    }
}