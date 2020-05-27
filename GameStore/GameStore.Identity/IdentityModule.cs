using Autofac;
using GameStore.Identity.CookieValidation;
using GameStore.Identity.Factories;
using GameStore.Identity.Factories.Interfaces;
using GameStore.Identity.PolicyProviders;
using GameStore.Identity.Requirements;
using GameStore.Identity.Requirements.Handlers;
using Microsoft.AspNetCore.Authorization;

namespace GameStore.Identity
{
    public class IdentityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthorizationPolicyProvider>()
                .As<IAuthorizationPolicyProvider>()
                .SingleInstance();

            builder.RegisterType<PermissionRequirementHandler>()
                .As<IAuthorizationHandler>()
                .SingleInstance();

            builder.RegisterType<ClaimsPrincipalFactory>()
                .As<IClaimsPrincipalFactory>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<CustomCookieAuthenticationEvents>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}