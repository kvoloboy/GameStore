using System.Threading.Tasks;
using GameStore.BusinessLayer.Models;
using GameStore.Common.Models;

namespace GameStore.BusinessLayer.Services.Interfaces
{
    public interface IBasketService
    {
        Task<Basket> GetBasketForUserAsync(string userId, string culture = Culture.En);
        Task<string> AddAsync(string gameKey, string customerId);
        Task UpdateQuantityAsync(string detailsId, short quantity);
        Task DeleteAsync(string detailsId);
    }
}