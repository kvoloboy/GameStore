
using GameStore.SeedingServices.Models;
using MongoDB.Bson;

namespace GameStore.SeedingServices.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        void SetKey(ObjectId productId, string key);
    }
}