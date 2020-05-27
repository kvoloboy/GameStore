using System.Threading.Tasks;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models.Identity;
using GameStore.Identity.Extensions;
using GameStore.Web.Models.ViewModels.PublisherViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameStore.Web.Filters
{
    public class IsAllowedUpdatePublisherFilter : IAsyncActionFilter
    {
        private readonly IPublisherService _publisherService;

        public IsAllowedUpdatePublisherFilter(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.IsInRole(DefaultRoles.Manager))
            {
                return;
            }

            const string getParameterName = "Id";
            const string postParameterName = "publisherViewModel";
            string publisherId;

            if (context.ActionArguments.TryGetValue(getParameterName, out var id))
            {
                publisherId = (string) id;
            }

            else
            {
                var publisherViewModel = (ModifyPublisherViewModel) context.ActionArguments[postParameterName];
                publisherId = publisherViewModel.Id;
            }

            var userId = context.HttpContext.User.GetId();
            var publisher = await _publisherService.GetByUserIdAsync(userId);

            if (publisher == null || publisher.Id != publisherId)
            {
                context.Result = new UnauthorizedResult();
            }

            await next();
        }
    }

    public class IsAllowedUpdatePublisherAttribute : TypeFilterAttribute
    {
        public IsAllowedUpdatePublisherAttribute()
            : base(typeof(IsAllowedUpdatePublisherFilter))
        {
        }
    }
}