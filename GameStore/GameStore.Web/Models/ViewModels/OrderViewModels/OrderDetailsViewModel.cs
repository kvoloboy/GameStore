namespace GameStore.Web.Models.ViewModels.OrderViewModels
{
    public class OrderDetailsViewModel
    {
        public string Id { get; set; }
        public string GameName { get; set; }
        public string GameImage { get; set; }
        public short Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
}