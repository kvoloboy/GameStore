using System;
using System.Linq;
using System.Threading.Tasks;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Models.Identity;
using GameStore.Identity.Attributes;
using GameStore.Identity.Extensions;
using GameStore.Identity.Factories.Interfaces;
using GameStore.Web.Models.ViewModels;
using GameStore.Web.Models.ViewModels.IdentityViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace GameStore.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IClaimsPrincipalFactory _principalFactory;
        private readonly ILogger<AccountController> _logger;
        private readonly IStringLocalizer<AccountController> _stringLocalizer;

        public AccountController(
            IUserService userService,
            IClaimsPrincipalFactory principalFactory,
            ILogger<AccountController> logger,
            IStringLocalizer<AccountController> stringLocalizer)
        {
            _userService = userService;
            _principalFactory = principalFactory;
            _logger = logger;
            _stringLocalizer = stringLocalizer;
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", registerViewModel);
            }

            var userDto = _userService.GetDefaultUserDto(User?.GetId(), registerViewModel.Email);

            try
            {
                await _userService.CreateAsync(userDto, registerViewModel.Password);
            }
            catch (InvalidServiceOperationException e)
            {
                var localizedMessage = _stringLocalizer[e.Message];
                ModelState.AddModelError(string.Empty, localizedMessage);

                return View("Register", registerViewModel);
            }

            await SignInUserAsync(User?.GetId());
            _logger.LogDebug($"Create new user with id: {User?.GetId()}");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("sign-in")]
        public IActionResult SignIn()
        {
            return View(new SignInViewModel());
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignInAsync(SignInViewModel signInViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("SignIn", signInViewModel);
            }

            var userId = await _userService.TrySignInAsync(signInViewModel.Email, signInViewModel.Password);

            if (!string.IsNullOrEmpty(userId))
            {
                await MergeUserActionsAsync(userId);
                await SignInUserAsync(userId);
            }
            else
            {
                const string signInKey = "Invalid attempt to sign in";
                ModelState.AddModelError(string.Empty, _stringLocalizer[signInKey]);

                return View("SignIn", signInViewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("sign-out")]
        public async Task<IActionResult> SignOutAsync(string returnUrl)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("ban")]
        [HasPermission(Permissions.Ban)]
        public IActionResult Ban(string userId)
        {
            var banTerms = Enum.GetNames(typeof(BanTerm));
            var localizedTerms = banTerms.Select(term => _stringLocalizer[term]).ToArray();
            var listItems = localizedTerms.Select((term, i) => new SelectListItem(term, banTerms[i]));

            var viewModel = new BanViewModel
            {
                UserId = userId,
                Terms = listItems
            };

            return View(viewModel);
        }

        [HttpPost("ban")]
        [HasPermission(Permissions.Ban)]
        public async Task<IActionResult> BanAsync(string userId, BanTerm banTerm)
        {
            await _userService.BanAsync(userId, banTerm);
            _logger.LogDebug($"Ban user: {userId} on term {banTerm.ToString()}");

            return RedirectToAction("Index", "Home");
        }

        private async Task MergeUserActionsAsync(string userId)
        {
            var guestId = User?.GetId();
            await _userService.MergeUserActionsAsync(guestId, userId);
        }

        private async Task SignInUserAsync(string userId)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = await _principalFactory.CreateAsync(userId);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}