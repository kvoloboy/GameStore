using GameStore.BusinessLayer.Sort.Options.Interfaces;

namespace GameStore.BusinessLayer.Sort.Factories.Interfaces
{
    public interface ISortOptionFactory<TEntity>
    {
        ISortOption<TEntity> Create(string sortOption);
    }
}