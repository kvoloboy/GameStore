using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Services.Interfaces
{
    public interface IInvoiceService
    {
        AppFile CreateInvoiceFile(string orderId, string userId, decimal total);
    }
}