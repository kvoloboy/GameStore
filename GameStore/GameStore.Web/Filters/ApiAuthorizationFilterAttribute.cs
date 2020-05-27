using System;
using GameStore.Identity.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameStore.Web.Filters
{
    public class ApiAuthorizationFilterAttribute : Attribute, IAuthorizationFilter
    {
        public string Permission { get; }

        public ApiAuthorizationFilterAttribute(string permission)
        {
            Permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.IsAllowedPermission(Permission))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}