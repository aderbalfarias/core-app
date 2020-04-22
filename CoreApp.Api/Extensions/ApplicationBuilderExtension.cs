using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApp.Api.Middlewares
{
    public static class ApplicationBuilderExtension
    {
        //public static IApplicationBuilder UseIf
        //(
        //    this IApplicationBuilder application, 
        //    bool condition,
        //    Func<IApplicationBuilder, IApplicationBuilder> action
        //)
        //{
        //    if (application == null)
        //        throw new ArgumentNullException(nameof(application));

        //    if (action == null)
        //        throw new ArgumentNullException(nameof(action));

        //    if (condition)
        //        application = action(application);

        //    return application;
        //}

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
