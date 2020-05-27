using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models.Identity;
using GameStore.Identity.Factories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GameStore.Identity.Factories
{
    public class ClaimsPrincipalFactory : IClaimsPrincipalFactory
    {
        private readonly IUserService _userService;

        public ClaimsPrincipalFactory(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ClaimsPrincipal> CreateAsync(string userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            var isBanned = user.BannedTo > DateTime.UtcNow;

            var permissionNames = user.Roles.SelectMany(r => r.Permissions).Distinct();
            var roles = user.Roles.Select(r => r.Name);

            var claims = isBanned
                ? permissionNames.Where(p => p != Permissions.CreateComment)
                    .Select(p => new Claim(CustomClaimTypes.Permission, p)).ToList()
                : permissionNames.Select(p => new Claim(CustomClaimTypes.Permission, p)).ToList();

            claims.Add(new Claim(CustomClaimTypes.Identifier, userId));
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimPrincipal = new ClaimsPrincipal(identity);

            return claimPrincipal;
        }
    }
}