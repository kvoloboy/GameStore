using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace GameStore.Web.Filters
{
    public class BenchmarkServicesFilter : IActionFilter
    {
        private readonly ILogger<BenchmarkServicesFilter> _logger;
        private readonly Stopwatch _stopwatch;

        public BenchmarkServicesFilter(ILogger<BenchmarkServicesFilter> logger)
        {
            _logger = logger;
            _stopwatch = new Stopwatch();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch.Start();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch.Stop();
            var path = context.HttpContext.Request.Path;
            _logger.LogInformation($"Execution \"{path}\" took {_stopwatch.ElapsedMilliseconds} ms");
        }
    }
}