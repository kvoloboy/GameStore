using GameStore.Web.WebServices.Payments.Interfaces;

namespace GameStore.Web.Factories.Interfaces
{
    public interface IPaymentStrategyFactory
    {
        IPaymentStrategy Create(string name);
    }
}