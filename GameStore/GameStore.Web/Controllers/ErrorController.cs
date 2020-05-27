using System;
using System.Net;
using GameStore.Web.Models.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Web.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var path = exceptionHandlerPathFeature.Path;
            var message = exceptionHandlerPathFeature.Error.Message;
            var viewModel = new ErrorViewModel
            {
                Path = path,
                Message = message
            };
            
            return View("Error", viewModel);
        }

        [Route("{statusCode}")]
        public IActionResult StatusCodeHandler(HttpStatusCode statusCode)
        {
            var name = Enum.GetName(typeof(HttpStatusCode), statusCode) ?? "Unrecognized";
            var value = (int) statusCode;
            var codeViewModel = new StatusCodeViewModel
            {
                Name = name,
                Value = value
            };
            
            return View("StatusCode", codeViewModel);
        }
    }
}