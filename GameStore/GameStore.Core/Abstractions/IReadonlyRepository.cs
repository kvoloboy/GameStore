using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GameStore.Core.Abstractions
{
    public interface IReadonlyRepository<T>
    {
        T FindSingle(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate = null);
        bool Any(Expression<Func<T, bool>> predicate);
    }
}