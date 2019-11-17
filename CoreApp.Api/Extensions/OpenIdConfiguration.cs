using CoreApp.Api.Options.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using System;
using System.Threading.Tasks;

namespace CoreApp.Api.Extensions
{
    public static class OpenIdConfiguration
    {
        /// <summary>
        ///     Setup OpenId Configuration
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static async Task InitializeAsync(this IServiceProvider services, IWebHostEnvironment environment)
        {
            // Create a new service scope to ensure the database context is correctly disposed when this methods returns.
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //var context = scope.ServiceProvider.GetRequiredService<DbContext>();
                //await context.Database.EnsureCreatedAsync();

                var manager = scope.ServiceProvider
                    .GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

                var openIdOptions = scope.ServiceProvider.GetRequiredService<OIDCAuthorizationServerOptions>();

                foreach (var client in openIdOptions.Clients)
                {
                    foreach (var descriptor in client.ApplicationDescriptors)
                    {
                        //var clientId = descriptor.ClientId;
                        descriptor.ClientId = client.ClientId;
                        descriptor.ClientSecret = client.ClientSecret;

                        if (await manager.FindByClientIdAsync(client.ClientId) != null)
                            continue;

                        await manager.CreateAsync(descriptor);
                    }
                }
            }

            return;
        }
    }
}
