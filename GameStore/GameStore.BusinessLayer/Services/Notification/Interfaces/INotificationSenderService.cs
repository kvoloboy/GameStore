using System.Threading.Tasks;
using GameStore.BusinessLayer.Models;

namespace GameStore.BusinessLayer.Services.Notification.Interfaces
{
    public interface INotificationSenderService<TInvoker> where TInvoker : class
    {
        Task NotifyAsync(NotificationContext<TInvoker> notificationContext);
    }
}