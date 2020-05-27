using GameStore.Core.Models.Identity;

namespace GameStore.Core.Models
{
    public class Rating
    {
        public string Id { get; set; }
        public int Value { get; set; }
        
        public string UserId { get; set; }
        public User User { get; set; }
        
        public string GameRootId { get; set; }
        public GameRoot GameRoot { get; set; }
    }
}