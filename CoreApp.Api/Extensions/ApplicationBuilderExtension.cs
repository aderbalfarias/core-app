using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Mime;

namespace CoreApp.Api.Middlewares
{
    public static class ApplicationBuilderExtension
    {
        /// <summary>
        ///     Executes the specific action if codition parameter is true
        ///     Used to conditionally add actions to the execution pipeline
        /// </summary>
        /// <param name="application">Application builder</param>
        /// <param name="condition">If it is true the action is executed</param>
        /// <param name="action">Action requested to be added to execution pipeline</param>
        /// <returns>Application builder</returns>
        public static IApplicationBuilder UseIf
        (
            this IApplicationBuilder application,
            bool condition,
            Func<IApplicationBuilder, IApplicationBuilder> action
        ) => application != null && action != null && condition
            ? action(application)
            : application;

        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder application)
        {
            application.UseHealthChecks("/ping");
            application.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    var result = JsonConvert.SerializeObject(new
                    {
                        status = report.Status.ToString(),
                        errors = report.Entries.Select(e => new
                        {
                            key = e.Key,
                            value = Enum.GetName(typeof(HealthStatus),
                            e.Value.Status)
                        })
                    });

                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(result);
                }
            });

            return application;
        }
    }
}
