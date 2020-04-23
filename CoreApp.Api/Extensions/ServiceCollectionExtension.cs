using Microsoft.Extensions.DependencyInjection;
using System;

namespace CoreApp.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        ///     Executes the specific action if codition parameter is true
        ///     Used to conditionally add actions to the servives
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="condition">If it is true the action is executed</param>
        /// <param name="action">Action requested to be added to servives</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddIf
        (
            this IServiceCollection services,
            bool condition,
            Func<IServiceCollection, IServiceCollection> action
        ) => services != null && action != null && condition
            ? action(services)
            : services;
    }
}