using System.Threading.Tasks;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Identity;
using GameStore.Identity.Factories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameStore.Web.Filters
{
    public class GuestConfigurationFilter : IAsyncAuthorizationFilter
    {
        private readonly IUserService _userService;
        private readonly IClaimsPrincipalFactory _claimsPrincipalFactory;

        public GuestConfigurationFilter(IUserService userService, IClaimsPrincipalFactory claimsPrincipalFactory)
        {
            _userService = userService;
            _claimsPrincipalFactory = claimsPrincipalFactory;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }

            var userId = await _userService.CreateGuestAsync();
            var claimsPrincipal = await _claimsPrincipalFactory.CreateAsync(userId);
            await context.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        }
    }
}