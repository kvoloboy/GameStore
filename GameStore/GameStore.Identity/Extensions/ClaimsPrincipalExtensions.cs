using System.Security.Claims;

namespace GameStore.Identity.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsAllowedPermission(this ClaimsPrincipal user, string permission)
        {
            var isAllowed = user.HasClaim(c => c.Type == CustomClaimTypes.Permission && c.Value == permission);

            return isAllowed;
        }

        public static string GetId(this ClaimsPrincipal user)
        {
            var userId = user.FindFirst(c => c.Type == CustomClaimTypes.Identifier)?.Value ?? string.Empty;

            return userId;
        }
    }
}