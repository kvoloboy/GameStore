using System;
using GameStore.BusinessLayer.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace GameStore.Web.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is BaseEntityNotFoundException)
            {
                context.Result = new NotFoundResult();

                context.ExceptionHandled = true;
            }

            else
            {
                var error = new
                {
                    Success = false,
                    Errors = new[] {context.Exception.Message}
                };

                context.Result = new ObjectResult(error)
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            _logger.LogError($"Error has occured.{context.Exception.Message}");
        }
    }
}