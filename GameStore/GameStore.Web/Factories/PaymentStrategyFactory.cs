using Autofac;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.WebServices.Payments.Interfaces;

namespace GameStore.Web.Factories
{
    public class PaymentStrategyFactory : IPaymentStrategyFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        public PaymentStrategyFactory(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }
        
        public IPaymentStrategy Create(string name)
        {
            var strategy = _lifetimeScope.ResolveNamed<IPaymentStrategy>(name);

            return strategy;
        }
    }
}