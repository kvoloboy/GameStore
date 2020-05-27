using System;
using System.Linq.Expressions;
using GameStore.BusinessLayer.Sort.Options.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Sort.Options
{
    public class PriceAscSortOption : ISortOption<GameRoot>
    {
        public SortDirection SortDirection => SortDirection.Ascending;
        public Expression<Func<GameRoot, object>> SortPropertyAccessor => root => root.Details.Price;
    }
}