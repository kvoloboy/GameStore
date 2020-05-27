using System.Threading.Tasks;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Identity.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.IdentityViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class ApiAccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IClaimsPrincipalFactory _principalFactory;

        public ApiAccountController(IUserService userService, IClaimsPrincipalFactory principalFactory)
        {
            _userService = userService;
            _principalFactory = principalFactory;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignInAsync(SignInViewModel viewModel)
        {
            var userId = await _userService.TrySignInAsync(viewModel.Email, viewModel.Password);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = await _principalFactory.CreateAsync(userId);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok();
        }
    }
}