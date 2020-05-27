using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BusinessLayer.DTO;

namespace GameStore.BusinessLayer.Services.Interfaces
{
    public interface IGameImageService
    {
        Task<IEnumerable<GameImageDto>> GetByGameKeyAsync(string gameKey);
        Task<GameImageDto> GetByIdAsync(string id);
        Task CreateAsync(GameImageDto image);
        Task DeleteAsync(string id);
    }
}