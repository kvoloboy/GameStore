using System;
using System.Security.Claims;
using System.Threading.Tasks;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models.Identity;
using GameStore.Identity.Extensions;
using GameStore.Identity.Factories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GameStore.Identity.CookieValidation
{
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private readonly IUserService _userService;
        private readonly IClaimsPrincipalFactory _claimsPrincipalFactory;

        public CustomCookieAuthenticationEvents(IUserService userService,
            IClaimsPrincipalFactory claimsPrincipalFactory)
        {
            _userService = userService;
            _claimsPrincipalFactory = claimsPrincipalFactory;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userId = context.Principal.GetId();
            var user = await _userService.GetByIdAsync(userId);

            if (user == null)
            {
                return;
            }

            var isBanned = user.BannedTo > DateTime.UtcNow;
            var canComment = context.Principal.IsAllowedPermission(Permissions.CreateComment);
            var shouldBanPrincipal = isBanned && canComment;

            if (!shouldBanPrincipal)
            {
                return;
            }
            
            var claimsPrincipal = await _claimsPrincipalFactory.CreateAsync(userId);
            ReplacePrincipal(context, claimsPrincipal);
        }
        
        private static void ReplacePrincipal(CookieValidatePrincipalContext context, ClaimsPrincipal principal)
        {
            context.ShouldRenew = true;
            context.ReplacePrincipal(principal);
        }
    }
}