using Microsoft.AspNetCore.Authorization;

namespace GameStore.Identity.Requirements
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }

        public string PermissionName { get; }
    }
}