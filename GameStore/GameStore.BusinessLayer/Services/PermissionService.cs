using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models.Identity;

namespace GameStore.BusinessLayer.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IAsyncReadonlyRepository<Permission> _permissionRepository;

        public PermissionService(IAsyncReadonlyRepository<Permission> permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            var permissions = await _permissionRepository.FindAllAsync();

            return permissions;
        }
    }
}