using Microsoft.AspNetCore.Authorization;

namespace GameStore.Identity.Attributes
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string permission)
            : base(permission)
        {
        }
    }
}