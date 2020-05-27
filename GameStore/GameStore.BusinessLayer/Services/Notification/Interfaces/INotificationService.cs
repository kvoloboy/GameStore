using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.BusinessLayer.Services.Notification.Interfaces
{
    public interface INotificationService<in TInvoker>
    {
        Task<IEnumerable<Core.Models.Notification.Notification>> GetNotificationMethodsAsync();
        Task SubscribeAsync(string userId, IEnumerable<string> notificationsId);
        Task UnsubscribeAsync(string userId);
        Task NotifyAsync(TInvoker invoker);
    }
}