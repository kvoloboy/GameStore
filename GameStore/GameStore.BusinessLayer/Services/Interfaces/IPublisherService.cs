using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BusinessLayer.DTO;
using GameStore.Common.Models;

namespace GameStore.BusinessLayer.Services.Interfaces
{
    public interface IPublisherService
    {
        Task CreateAsync(ModifyPublisherDto publisherDto);
        Task UpdateAsync(ModifyPublisherDto publisherDto);
        Task DeleteAsync(string id);
        Task<PublisherDto> GetByIdAsync(string id, string culture);
        Task<ModifyPublisherDto> GetByIdAsync(string id);
        Task<PublisherDto> GetByUserIdAsync(string userId, string culture = Culture.En);
        Task<PublisherDto> GetByCompanyAsync(string companyName, string culture = Culture.En);
        Task<IEnumerable<PublisherDto>> GetAllAsync(string culture = Culture.En);
    }
}