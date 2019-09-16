using CoreApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CoreApp.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, 
            ILogger<ExceptionMiddleware> logger,
            IOptions<AppSettings> appConfigKeys)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                logger.LogError($"Exception logged in {httpContext?.Request?.Path}, Error: {e}");
                await HandleExceptionAsync(httpContext, appConfigKeys.Value);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, AppSettings appConfigKeys)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            
            var json = JsonConvert.SerializeObject(new
            {
                context.Response.StatusCode,
                Message = appConfigKeys.ResponseErrorMessage
            });

            return context.Response.WriteAsync(json);
        }
    }
}
