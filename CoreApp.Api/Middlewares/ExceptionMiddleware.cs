using CoreApp.Api.Extesions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CoreApp.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<ConfigKeys> _appConfigKeys;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IOptions<ConfigKeys> appConfigKeys)
        {
            _logger = logger;
            _next = next;
            _appConfigKeys = appConfigKeys;
        }

        public async Task Invoke(HttpContext httpContext, IHostingEnvironment hostingEnvironment)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception logged in {context?.Request?.Path}");
                await HandleExceptionAsync(httpContext);
            }
        }

        private Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var newResponse = new
            {
                context.Response.StatusCode,
                Message = "Internal Server Error Test"
            };

            return context.Response.WriteAsync(newResponse.ToString());
        }
    }
}
