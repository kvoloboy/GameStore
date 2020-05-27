using System;
using GameStore.Core.Models.Identity;

namespace GameStore.Core.Models
{
    public class Comment
    {
        public Comment()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public string QuoteText { get; set; }
        public bool IsDeleted { get; set; }
        public string ParentId { get; set; }
        public Comment Parent { get; set; }
        public string GameRootId { get; set; }
        public GameRoot GameRoot { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}