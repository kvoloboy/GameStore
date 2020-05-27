namespace GameStore.Web.Models.ViewModels.PaymentViewModels
{
    public class PaymentDetailsViewModel
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public decimal Total { get; set; }
    }
}