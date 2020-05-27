namespace GameStore.Core.Models
{
    public enum OrderState
    {
        New = 1,
        Ordered,
        Pending,
        Payed,
        Shipped,
        Canceled,
        Closed
    }
}