namespace GameStore.Core.Abstractions
{
    public interface IRepositoryFactory
    {
        TRepository GetRepository<TRepository>();
    }
}