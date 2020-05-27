using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models.Identity;

namespace GameStore.BusinessLayer.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAsyncRepository<Role> _roleRepository;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _roleRepository = unitOfWork.GetRepository<IAsyncRepository<Role>>();
        }

        public async Task CreateAsync(RoleDto roleDto)
        {
            if (roleDto == null)
            {
                throw new InvalidServiceOperationException("Is null dto");
            }

            var role = GetRole(roleDto);
            await ValidateRoleExistingByName(role.Name);

            await _roleRepository.AddAsync(role);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(RoleDto roleDto)
        {
            if (roleDto == null)
            {
                throw new InvalidServiceOperationException("Is null dto");
            }

            var role = GetRole(roleDto);
            await ValidateRoleName(role.Id, role.Name);

            await _roleRepository.UpdateAsync(role);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var exist = await _roleRepository.AnyAsync(r => r.Id == id);

            if (!exist)
            {
                throw new EntityNotFoundException<Role>(id);
            }

            await _roleRepository.DeleteAsync(id);
            await _unitOfWork.CommitAsync();
        }

        public async Task<Role> GetByIdAsync(string id)
        {
            var role = await _roleRepository.FindSingleAsync(r => r.Id == id);

            return role;
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidServiceOperationException("Is empty name");
            }

            var role = await _roleRepository.FindSingleAsync(r => r.Name == name);

            return role;
        }

        public async Task<IEnumerable<Role>> GetByNamesAsync(IEnumerable<string> names)
        {
            if (names == null || !names.Any())
            {
                throw new InvalidServiceOperationException("Is empty name");
            }

            var roles = await _roleRepository.FindAllAsync(r => names.Contains(r.Name));

            return roles;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            var roles = await _roleRepository.FindAllAsync();

            return roles;
        }

        public bool CanDelete(string roleName)
        {
            var canDelete = roleName != DefaultRoles.Admin;

            return canDelete;
        }

        private async Task ValidateRoleName(string id, string name)
        {
            var existingRole = await GetByIdAsync(id);

            if (existingRole == null)
            {
                throw new EntityNotFoundException<Role>(id);
            }

            var isChangedName = name != existingRole.Name;

            if (isChangedName)
            {
                await ValidateRoleExistingByName(name);
            }
        }

        private static Role GetRole(RoleDto roleDto)
        {
            var role = new Role
            {
                Id = roleDto.Id,
                Name = roleDto.Name
            };
            role.RolePermissions = roleDto.Permissions.Select(permission => new RolePermission
            {
                PermissionId = permission,
                RoleId = role.Id
            }).ToList();

            return role;
        }

        private async Task ValidateRoleExistingByName(string name)
        {
            var alreadyExist = await _roleRepository.AnyAsync(r => r.Name == name);

            if (alreadyExist)
            {
                throw new EntityExistsWithKeyValueException<Role>(nameof(Role.Name), name);
            }
        }
    }
}