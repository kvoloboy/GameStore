using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GameStore.Core.Abstractions
{
    public interface IAsyncRepository<T> : IAsyncReadonlyRepository<T>
        where T : class
    {
        Task AddAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteAsync(string id);
    }
}