using System.Threading.Tasks;
using GameStore.Core.Abstractions;
using GameStore.DataAccess.Sql.Context;

namespace GameStore.DataAccess.Sql.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IRepositoryFactory _repositoryFactory;

        public UnitOfWork(AppDbContext dbContext, IRepositoryFactory repositoryFactory)
        {
            _dbContext = dbContext;
            _repositoryFactory = repositoryFactory;
        }

        public TRepository GetRepository<TRepository>()
        {
            return _repositoryFactory.GetRepository<TRepository>();
        }
        
        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}