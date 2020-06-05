using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.BusinessLayer.Services.Notification.Interfaces;
using GameStore.Core.Models;
using GameStore.Core.Models.Identity;
using GameStore.Identity.Attributes;
using GameStore.Identity.Extensions;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models;
using GameStore.Web.Models.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace GameStore.Web.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly INotificationService<Order> _notificationService;
        private readonly IPublisherService _publisherService;
        private readonly IAsyncViewModelFactory<string, ModifyUserViewModel> _modifyUserViewModelFactory;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService,
            INotificationService<Order> notificationService,
            IPublisherService publisherService,
            IAsyncViewModelFactory<string, ModifyUserViewModel> modifyOrderViewModelFactory,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _notificationService = notificationService;
            _modifyUserViewModelFactory = modifyOrderViewModelFactory;
            _publisherService = publisherService;
            _logger = logger;
        }

        [HttpGet]
        [HasPermission(Permissions.ReadUsers)]
        public async Task<IActionResult> IndexAsync()
        {
            var users = await _userService.GetAllAsync();
            var userViewModels = users.Select(u => new UserViewModel
            {
                Id = u.Id,
                Email = u.Email,
                Roles = string.Join(", ", u.Roles.Select(r => r.Name))
            });

            return View("Index", userViewModels);
        }

        [HttpGet("setup-roles")]
        [HasPermission(Permissions.SetupRoles)]
        public async Task<IActionResult> SetupRolesAsync(string userId)
        {
            var viewModel = await _modifyUserViewModelFactory.CreateAsync(userId);

            return View("SetupRoles", viewModel);
        }

        [HttpPost("setup-roles")]
        [HasPermission(Permissions.SetupRoles)]
        public async Task<IActionResult> SetupRolesAsync(ModifyUserViewModel modifyUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _userService.UpdateRolesAsync(modifyUserViewModel.Id, modifyUserViewModel.SelectedRoles);
            _logger.LogDebug($"Update user roles. User id : {modifyUserViewModel.Id}");

            return RedirectToAction(nameof(IndexAsync));
        }

        [HttpGet("assign-to-publisher")]
        [HasPermission(Permissions.SetupRoles)]
        public async Task<IActionResult> AssignToPublisherAsync(string userId)
        {
            var publishers = (await _publisherService.GetAllAsync()).Where(p => p.CanBeUsed);
            var viewModel = new AssignToPublisherViewModel
            {
                UserId = userId,
                Publishers = publishers.Select(p => new SelectListItem
                {
                    Text = p.CompanyName,
                    Value = p.Id
                })
            };

            return View("AssignToPublisher", viewModel);
        }

        [HttpPost("assign-to-publisher")]
        [HasPermission(Permissions.SetupRoles)]
        public async Task<IActionResult> AssignToPublisherAsync(AssignToPublisherViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _userService.AssignToPublisherAsync(viewModel.UserId, viewModel.PublisherId);
            _logger.LogDebug(
                $"Assign user to publisher. User id: {viewModel.UserId}. Publisher id: :{viewModel.PublisherId}");

            return RedirectToAction(nameof(IndexAsync));
        }

        [HttpGet("personal-area")]
        [HasPermission(Permissions.ReadPersonalOrders)]
        public IActionResult GetPersonalArea()
        {
            return View("PersonalArea");
        }

        [HttpGet("subscribe")]
        [HasPermission(Permissions.SubscribeOnNotifications)]
        public async Task<IActionResult> SubscribeOnNotificationsAsync()
        {
            var notificationMethods = await _notificationService.GetNotificationMethodsAsync();
            var user = await _userService.GetByIdAsync(User?.GetId());

            var viewModel = new SubscribeOnNotificationsViewModel
            {
                NotificationMethods = notificationMethods.Select(method => new ListItem
                {
                    Id = method.Id,
                    Name = method.Name
                }),
                SelectedNotifications = user.SelectedNotifications
            };

            return View("SubscribeOnNotifications", viewModel);
        }

        [HttpPost("subscribe")]
        [HasPermission(Permissions.SubscribeOnNotifications)]
        public async Task<IActionResult> SubscribeOnNotificationsAsync(IEnumerable<string> notifications)
        {
            var userId = User?.GetId();
            await _notificationService.SubscribeAsync(userId, notifications);

            return RedirectToAction("Index", "Home");
        }
    }
}
