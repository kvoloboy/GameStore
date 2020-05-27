using System;

namespace GameStore.Core.Models
{
    public class Visit
    {
        public Visit()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public string Id { get; set; }
        public string GameRootId { get; set; }
        public GameRoot GameRoot { get; set; }
        public long Value { get; set; }
    }
}