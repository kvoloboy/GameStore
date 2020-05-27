namespace GameStore.BusinessLayer.DTO
{
    public class OrderDetailsDto
    {
        public string Id { get; set; }
        public short Quantity { get; set; } = 1;
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string OrderId { get; set; }
        public string GameId { get; set; }
        public GameDto Game { get; set; }
    }
}