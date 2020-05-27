using Autofac;
using GameStore.Core.Abstractions;

namespace GameStore.DataAccess.Sql.Factories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        public RepositoryFactory(ILifetimeScope scope)
        {
            _lifetimeScope = scope;
        }

        public TRepository GetRepository<TRepository>()
        {
            return _lifetimeScope.Resolve<TRepository>();
        }
    }
}