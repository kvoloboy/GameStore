using Autofac;
using GameStore.BusinessLayer.Factories.Interfaces;
using GameStore.BusinessLayer.Models;
using GameStore.BusinessLayer.Services.Notification.Interfaces;

namespace GameStore.BusinessLayer.Factories
{
    public class NotificationSenderServiceFactory<TInvoker> : INotificationSenderServiceFactory<TInvoker>
        where TInvoker : class
    {
        private readonly ILifetimeScope _lifetimeScope;

        public NotificationSenderServiceFactory(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public INotificationSenderService<TInvoker> Create(NotificationMethod method)
        {
            var service = _lifetimeScope.ResolveKeyed<INotificationSenderService<TInvoker>>(method);

            return service;
        }
    }
}