namespace GameStore.Core.Abstractions
{
    public interface IRepository<T> : IReadonlyRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(string id);
    }
}
