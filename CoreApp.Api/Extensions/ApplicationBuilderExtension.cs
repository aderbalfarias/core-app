using Microsoft.AspNetCore.Builder;
using System;

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
    }
}
