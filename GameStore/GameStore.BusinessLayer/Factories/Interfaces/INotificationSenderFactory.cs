using GameStore.BusinessLayer.Models;
using GameStore.BusinessLayer.Services.Notification.Interfaces;

namespace GameStore.BusinessLayer.Factories.Interfaces
{
    public interface INotificationSenderServiceFactory<TInvoker>
        where TInvoker : class
    {
        INotificationSenderService<TInvoker> Create(NotificationMethod method);
    }
}