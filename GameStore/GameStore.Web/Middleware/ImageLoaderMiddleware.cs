using System;
using System.Linq;
using System.Threading.Tasks;
using GameStore.BusinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GameStore.Web.Middleware
{
    public class ImageLoaderMiddleware
    {
        private const string ExpectedPath = "/images/1/get-with-middleware";

        private readonly RequestDelegate _next;

        public ImageLoaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IGameImageService gameImageService)
        {
            var path = context.Request.Path.Value;

            if (!path.Contains(ExpectedPath))
            {
                await _next(context);
                
                return;
            }

            const string id = "1";
            var image = await gameImageService.GetByIdAsync(id);
            context.Items["game-image"] = image;

            await _next(context);
        }
    }
}