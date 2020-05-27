using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BusinessLayer.DTO;
using GameStore.Core.Models.Identity;

namespace GameStore.BusinessLayer.Services.Interfaces
{
    public interface IRoleService
    {
        Task CreateAsync(RoleDto roleDto);
        Task UpdateAsync(RoleDto roleDto);
        Task DeleteAsync(string id);
        Task<Role> GetByIdAsync(string id);
        Task<Role> GetByNameAsync(string name);
        Task<IEnumerable<Role>> GetByNamesAsync(IEnumerable<string> names);
        Task<IEnumerable<Role>> GetAllAsync();
        bool CanDelete(string roleName);
    }
}