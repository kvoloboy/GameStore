using Autofac;
using GameStore.BusinessLayer.Sort.Factories.Interfaces;
using GameStore.BusinessLayer.Sort.Options.Interfaces;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Sort.Factories
{
    public class GameSortOptionFactory : ISortOptionFactory<GameRoot>
    {
        private readonly ILifetimeScope _lifetimeScope;

        public GameSortOptionFactory(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public ISortOption<GameRoot> Create(string sortOption)
        {
            var option = _lifetimeScope.ResolveNamed<ISortOption<GameRoot>>(sortOption);

            return option;
        }
    }
}