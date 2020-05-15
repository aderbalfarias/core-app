using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.Api.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            try
            {
                var request = httpContext.Request;

                if (request.Path.StartsWithSegments(new PathString("/api")))
                {
                    await _next(httpContext);

                    //var requestBody = await ReadRequestBody(request);
                    //var originalBodyStream = httpContext.Response.Body;

                    //using (var responseBody = new MemoryStream())
                    //{
                    //    //var startAction = DateTime.Now;

                    //    // Execution of the request when call next
                    //    var response = httpContext.Response;
                    //    response.Body = responseBody;
                    //    await _next(httpContext);

                    //    //var endAction = DateTime.Now;
                    //    //var ResposeBody = await ReadResponseBody(response);

                    //    await responseBody.CopyToAsync(originalBodyStream);
                    //}

                    //var resposeCode = httpContext.Response.StatusCode.ToString();
                    //var urlRequest = $"{httpContext.Request.Scheme}://" +
                    //    $"{httpContext.Request.Host}{httpContext.Request.Path}{httpContext.Request.QueryString}";
                    //var methodCalled = httpContext.Request.Method;
                }
                else
                {
                    await _next(httpContext);
                }
            }
            catch (Exception e)
            {
                logger.LogError($"Exception at {httpContext?.Request?.Path}, Error: {e}");
                await _next(httpContext);
            }
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var bodyAsText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }
    }
}