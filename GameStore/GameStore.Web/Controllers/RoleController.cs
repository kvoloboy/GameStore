using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models.Identity;
using GameStore.Identity.Attributes;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.RoleViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameStore.Web.Controllers
{
    [Route("role")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IAsyncViewModelFactory<ModifyRoleViewModel, ModifyRoleViewModel> _modifyRoleViewModelFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleController> _logger;

        public RoleController(
            IAsyncViewModelFactory<ModifyRoleViewModel, ModifyRoleViewModel> modifyRoleViewModelFactory,
            IRoleService roleService,
            IMapper mapper,
            ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _modifyRoleViewModelFactory = modifyRoleViewModelFactory;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [HasPermission(Permissions.ReadRoles)]
        public async Task<IActionResult> IndexAsync()
        {
            var roles = await _roleService.GetAllAsync();

            var viewModels = roles.Select(r =>
            {
                var roleViewModel = _mapper.Map<RoleViewModel>(r);
                roleViewModel.CanDelete = _roleService.CanDelete(roleViewModel.Name);

                return roleViewModel;
            });

            return View("Index", viewModels);
        }

        [HttpGet("create-role")]
        [HasPermission(Permissions.CreateRole)]
        public async Task<IActionResult> CreateAsync()
        {
            var viewModel = await _modifyRoleViewModelFactory.CreateAsync(new ModifyRoleViewModel());

            return View("Create", viewModel);
        }

        [HttpPost("create-role")]
        [HasPermission(Permissions.CreateRole)]
        public async Task<IActionResult> CreateAsync(ModifyRoleViewModel modifyRoleViewModel)
        {
            if (!ModelState.IsValid)
            {
                var pulledViewModel = await _modifyRoleViewModelFactory.CreateAsync(modifyRoleViewModel);

                return View("Create", pulledViewModel);
            }

            var roleDto = _mapper.Map<RoleDto>(modifyRoleViewModel);

            try
            {
                await _roleService.CreateAsync(roleDto);
            }
            catch (InvalidServiceOperationException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                var pulledViewModel = await _modifyRoleViewModelFactory.CreateAsync(modifyRoleViewModel);

                return View("Create", pulledViewModel);
            }

            _logger.LogDebug($"Create new role with name {roleDto.Name}");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("update-role")]
        [HasPermission(Permissions.UpdateRole)]
        public async Task<IActionResult> UpdateAsync(string id)
        {
            var role = await _roleService.GetByIdAsync(id);
            var viewModel = _mapper.Map<ModifyRoleViewModel>(role);
            var pulledViewModel = await _modifyRoleViewModelFactory.CreateAsync(viewModel);

            return View("Update", pulledViewModel);
        }

        [HttpPost("update-role")]
        [HasPermission(Permissions.UpdateRole)]
        public async Task<IActionResult> UpdateAsync(ModifyRoleViewModel modifyRoleViewModel)
        {
            if (!ModelState.IsValid)
            {
                var pulledViewModel = await _modifyRoleViewModelFactory.CreateAsync(modifyRoleViewModel);

                return View("Update", pulledViewModel);
            }

            var roleDto = _mapper.Map<RoleDto>(modifyRoleViewModel);

            try
            {
               await _roleService.UpdateAsync(roleDto);
            }
            catch (InvalidServiceOperationException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                var pulledViewModel = await _modifyRoleViewModelFactory.CreateAsync(modifyRoleViewModel);

                return View("Update", pulledViewModel);
            }

            _logger.LogDebug($"Update role with id {roleDto.Id}");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost("delete-role")]
        [HasPermission(Permissions.DeleteRole)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _roleService.DeleteAsync(id);

            return RedirectToAction("Index", "Home");
        }
    }
}