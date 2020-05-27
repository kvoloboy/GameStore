namespace GameStore.BusinessLayer.DTO
{
    public class GameImageDto
    {
        public string Id { get; set; }
        public string GameKey { get; set; }
        public byte[] Content  { get; set; }
        public string ContentType { get; set; }
    }
}