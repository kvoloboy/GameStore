using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.Core.Models.Identity;

namespace GameStore.BusinessLayer.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<IEnumerable<Permission>> GetAllAsync();
    }
}