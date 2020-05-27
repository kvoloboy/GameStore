using System;
using System.Linq.Expressions;
using GameStore.BusinessLayer.Sort.Options.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Sort.Options
{
    public class MostCommentedSortOption : ISortOption<GameRoot>
    {
        public SortDirection SortDirection => SortDirection.Descending;
        public Expression<Func<GameRoot, object>> SortPropertyAccessor => root => root.Comments.Count;
    }
}