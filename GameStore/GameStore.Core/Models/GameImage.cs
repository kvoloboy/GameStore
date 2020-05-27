namespace GameStore.Core.Models
{
    public class GameImage
    {
        public string Id { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
        
        public string GameRootId { get; set; }
        public GameRoot GameRoot { get; set; }
    }
}