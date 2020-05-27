using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BusinessLayer.DTO;

namespace GameStore.BusinessLayer.Services.Interfaces
{
    public interface IPlatformService
    {
        Task CreateAsync(PlatformDto platformDto);
        Task UpdateAsync(PlatformDto platformDto);
        Task DeleteAsync(string id);
        Task<IEnumerable<PlatformDto>> GetAllAsync();
        Task<PlatformDto> GetByIdAsync(string id);
    }
}