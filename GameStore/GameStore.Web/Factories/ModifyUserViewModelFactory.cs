using System.Linq;
using System.Threading.Tasks;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models;
using GameStore.Web.Models.ViewModels.UserViewModels;

namespace GameStore.Web.Factories
{
    public class ModifyUserViewModelFactory : IAsyncViewModelFactory<string, ModifyUserViewModel>
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public ModifyUserViewModelFactory(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }
        
        public async Task<ModifyUserViewModel> CreateAsync(string model)
        {
            var userDto = await _userService.GetByIdAsync(model);
            var roles = await _roleService.GetAllAsync();
            
            var viewModel = new ModifyUserViewModel
            {
                Id = userDto.Id,
                SelectedRoles = userDto.Roles.Select(r => r.Id)
            };

            viewModel.Roles = roles.Select(r => new ListItem
            {
                Id = r.Id,
                Name = r.Name,
                Selected = viewModel.SelectedRoles.Contains(r.Id)
            });

            return viewModel;
        }
    }
}