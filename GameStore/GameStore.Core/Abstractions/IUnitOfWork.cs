using System.Threading.Tasks;

namespace GameStore.Core.Abstractions
{
    public interface IUnitOfWork
    {
        TRepository GetRepository<TRepository>();
        Task<int> CommitAsync();
    }
}