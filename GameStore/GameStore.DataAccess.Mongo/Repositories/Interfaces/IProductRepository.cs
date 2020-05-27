using System.Threading.Tasks;
using GameStore.Core.Abstractions;
using GameStore.DataAccess.Mongo.Models;

namespace GameStore.DataAccess.Mongo.Repositories.Interfaces
{
    public interface IProductRepository : IAsyncReadonlyRepository<Product>
    {
        Task UpdateUnitsInStockAsync(string key, short unitsInStock);
        Task UpdateKeyAsync(string productId, string key);
    }
}