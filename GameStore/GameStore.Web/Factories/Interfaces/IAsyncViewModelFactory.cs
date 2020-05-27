using System.Threading.Tasks;

namespace GameStore.Web.Factories.Interfaces
{
    public interface IAsyncViewModelFactory<in TModel, TViewModel>
    {
        Task<TViewModel> CreateAsync(TModel model);
    }
}