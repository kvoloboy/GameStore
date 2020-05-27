using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.Core.Models;

namespace GameStore.Common.Decorators.Interfaces
{
    public interface IPublisherDecorator
    {
        Task AddAsync(Publisher publisher);
        Task UpdateAsync(Publisher publisher);
        Task DeleteAsync(string id);
        Task<Publisher> GetByIdAsync(string id);
        Task<Publisher> GetByUserIdAsync(string userId);
        Task<Publisher> GetByCompanyAsync(string companyName);
        Task<IEnumerable<Publisher>> GetAllAsync();
        Task<bool> IsExistByIdAsync(string id);
    }
}