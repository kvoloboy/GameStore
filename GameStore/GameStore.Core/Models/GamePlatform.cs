namespace GameStore.Core.Models
{
    public class GamePlatform
    {
        public string GameRootId { get; set; }
        public GameRoot GameRoot { get; set; }
        
        public string PlatformId { get; set; }
        public Platform Platform { get; set; }
    }
}