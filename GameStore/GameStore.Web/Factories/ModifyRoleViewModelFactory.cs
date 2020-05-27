using System.Linq;
using System.Threading.Tasks;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models;
using GameStore.Web.Models.ViewModels.RoleViewModels;

namespace GameStore.Web.Factories
{
    public class ModifyRoleViewModelFactory : IAsyncViewModelFactory<ModifyRoleViewModel, ModifyRoleViewModel>
    {
        private readonly IPermissionService _permissionService;

        public ModifyRoleViewModelFactory(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task<ModifyRoleViewModel> CreateAsync(ModifyRoleViewModel model)
        {
            var permissions = await _permissionService.GetAllAsync();
            var listItems = permissions.Select(p => new ListItem
            {
                Id = p.Id,
                Name = p.Value,
                Selected = model.SelectedPermissions.Contains(p.Id)
            });

            model.Permissions = listItems;

            return model;
        }
    }
}