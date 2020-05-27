using System.Threading.Tasks;

namespace GameStore.Common.Pipeline.Interfaces
{
    public interface IPipeline <TOutput>
    {
        Task<TOutput> ExecuteAsync();
    }
}