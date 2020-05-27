using System.Threading.Tasks;
using GameStore.BusinessLayer.DTO;

namespace GameStore.BusinessLayer.Services.Interfaces
{
    public interface IRatingService
    {
        Task CreateOrUpdateAsync(string gameId, string userId, int rating);
        Task<RatingDto> GetForGameAsync(string gameId);
    }
}