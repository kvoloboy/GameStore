using System.Threading.Tasks;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models.Identity;
using GameStore.Identity.Extensions;
using GameStore.Web.Models.ViewModels.GameViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameStore.Web.Filters
{
    public class IsAllowedUpdateGameFilter : IAsyncActionFilter
    {
        private readonly IPublisherService _publisherService;
        private readonly IGameService _gameService;

        public IsAllowedUpdateGameFilter(IPublisherService publisherService, IGameService gameService)
        {
            _publisherService = publisherService;
            _gameService = gameService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.IsInRole(DefaultRoles.Manager))
            {
                return;
            }

            const string getParameterName = "Id";
            const string postParameterName = "modifyGameViewModel";
            string gameId;

            if (context.ActionArguments.TryGetValue(getParameterName, out var id))
            {
                gameId = (string) id;
            }

            else
            {
                var modifyGameViewModel = (ModifyGameViewModel) context.ActionArguments[postParameterName];
                gameId = modifyGameViewModel.Id;
            }

            var userId = context.HttpContext.User.GetId();
            var publisher = await _publisherService.GetByUserIdAsync(userId);

            if (publisher == null)
            {
                context.Result = new UnauthorizedResult();

                return;
            }

            var game = await _gameService.GetByIdAsync(gameId);

            if (game.PublisherEntityId != publisher.Id)
            {
                context.Result = new UnauthorizedResult();
            }

            await next();
        }
    }

    public class IsAllowedUpdateGameAttribute : TypeFilterAttribute
    {
        public IsAllowedUpdateGameAttribute()
            : base(typeof(IsAllowedUpdateGameFilter))
        {
        }
    }
}