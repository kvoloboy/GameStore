using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.Common.Models;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;

namespace GameStore.Common.Decorators.Interfaces
{
    public interface IGameDecorator : IAsyncRepository<GameRoot>
    {
        Task<IEnumerable<GameRoot>> FindAllAsync(GameFilterData filterData);
        Task UpdateUnitsInStockAsync(string key, short newValue);
        Task<int> CountAsync();
    }
}