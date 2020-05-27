using System.Threading.Tasks;

namespace GameStore.Common.Aggregators.Interfaces
{
    public interface IAggregator<in TInput, TResult>
    {
        Task<TResult> FindAllAsync(TInput input);
    }
}