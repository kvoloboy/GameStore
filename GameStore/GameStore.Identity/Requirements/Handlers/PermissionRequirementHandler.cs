using System.Threading.Tasks;
using GameStore.Identity.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace GameStore.Identity.Requirements.Handlers
{
    public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var user = context.User;

            if (user.IsAllowedPermission(requirement.PermissionName))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}