using System.Collections.Generic;
using GameStore.Core.Models.Identity;

namespace GameStore.BusinessLayer.Models
{
    public abstract class NotificationContext<TInvoker>
        where TInvoker : class
    {
        public TInvoker Invoker { get; set; }
        public IEnumerable<User> Subscribers { get; set; }
    }
}