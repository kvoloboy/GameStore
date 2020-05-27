using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace GameStore.Web.Filters
{
    public class RequestIpFilter : IResourceFilter
    {
        private readonly ILogger<RequestIpFilter> _logger;

        public RequestIpFilter(ILogger<RequestIpFilter> logger)
        {
            _logger = logger;
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
            var userName = context.HttpContext.User.Identity.Name ?? string.Empty;
            var message = $"User {userName} ip - {ip}";
            _logger.LogDebug(message);
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}