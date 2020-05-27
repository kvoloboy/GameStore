using System.Threading.Tasks;
using GameStore.BusinessLayer.DTO;
using Microsoft.AspNetCore.Http;

namespace GameStore.Web.Middleware
{
    public class ImageWriterMiddleware
    {
        private readonly RequestDelegate _next;

        public ImageWriterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!(context.Items["game-image"] is GameImageDto image))
            {
                await _next.Invoke(context);
                
                return;
            }

            await context.Response.Body.WriteAsync(image.Content, 0, image.Content.Length);
        }
    }
}