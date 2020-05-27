using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Models;
using GameStore.Common.Models;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Services.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<GameDto>> GetAllAsync(string culture = Culture.En);
        Task<string> GenerateKeyAsync(string gameName, string separator);
        Task<PageList<IEnumerable<GameDto>>> GetPageListAsync(GameFilterData filterData, string culture = Culture.En);
        Task<GameDto> GetByIdAsync(string id, string culture);
        Task<ModifyGameDto> GetByIdAsync(string id);
        Task<GameDto> GetByKeyAsync(string key, string culture = Culture.En);
        Task<IEnumerable<GameDto>> GetByGenreAsync(string genreId);
        Task<IEnumerable<GameDto>> GetByPublisherAsync(string publisherId);
        Task CreateAsync(ModifyGameDto gameDto);
        Task UpdateAsync(ModifyGameDto gameDto);
        Task IncrementVisitsCountAsync(string gameKey);
        Task DeleteAsync(string id);
        decimal ComputePriceWithDiscount(decimal price, decimal discount);
        AppFile GetFile(string gameKey);
        Task<int> CountAsync();
        Task<GameDto> GetGameDtoAsync(GameRoot gameRoot, string culture, bool includePublisher = false);
    }
}