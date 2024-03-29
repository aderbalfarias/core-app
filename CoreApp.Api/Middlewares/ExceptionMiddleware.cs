﻿using CoreApp.Domain.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

namespace CoreApp.Api.Middlewares;

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
            logger.LogError(e, $"Exception logged at {httpContext?.Request?.Path}");
            await HandleExceptionAsync(httpContext, appConfigKeys.Value);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, AppSettings appConfigKeys)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)HttpStatusCode.InternalServerError;

        await response.WriteAsync(JsonConvert.SerializeObject(new
        {
            Message = appConfigKeys.ResponseErrorMessage
        }));
    }
}

