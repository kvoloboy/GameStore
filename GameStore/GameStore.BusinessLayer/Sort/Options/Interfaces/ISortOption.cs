using System;
using System.Linq.Expressions;
using GameStore.Common.Models;

namespace GameStore.BusinessLayer.Sort.Options.Interfaces
{
    public interface ISortOption<TEntity>
    {
        public SortDirection SortDirection { get; }
        public Expression<Func<TEntity, object>> SortPropertyAccessor { get; }
    }
}