using CoreApp.Api.Options.Authorization;
using CoreApp.Data.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;

namespace CoreApp.Api.Extensions
{
    public static class OpenIdConfiguration
    {
        /// <summary>
        ///     Setup OpenIddict Configuration
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static void OpenIdInitialize(this IServiceCollection services, IWebHostEnvironment environment)
        {
            // Create a new service scope to ensure the database context is correctly disposed when this methods returns.
            using (var scope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DemoContext>();
                context.Database.EnsureCreatedAsync();

                var manager = scope.ServiceProvider
                    .GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

                var openIdOptions = scope.ServiceProvider.GetRequiredService<OIDCAuthorizationServerOptions>();

                foreach (var client in openIdOptions.Clients)
                {
                    foreach (var descriptor in client.ApplicationDescriptors)
                    {
                        descriptor.ClientId = client.ClientId;
                        descriptor.ClientSecret = client.ClientSecret;

                        if (manager.FindByClientIdAsync(client.ClientId) != null)
                            continue;

                        manager.CreateAsync(descriptor);
                    }
                }
            }
        }

        public static void OpenIddict(this IServiceCollection services)
        {
            // Register the Identity services.
            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<PrimaryContext>()
            //    .AddDefaultTokenProviders();

            // Register the OpenIddict services.
            services.AddOpenIddict()
                .AddCore(options =>
                {
                    // Configure OpenIddict to use the Entity Framework Core stores and entities.
                    options.UseEntityFrameworkCore()
                        .UseDbContext<DemoContext>();
                })

                .AddServer(options =>
                {
                    // Register the ASP.NET Core MVC binder used by OpenIddict.
                    // Note: if you don't call this method, you won't be able to
                    // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                    options.UseMvc();

                    // Enable the token endpoint (required to use the password flow).
                    options.EnableTokenEndpoint("/connect/token");

                    // Allow client applications to use the grant_type=password flow.
                    options.AllowClientCredentialsFlow();

                    // During development, you can disable the HTTPS requirement.
                    options.DisableHttpsRequirement();

                    // Accept token requests that don't specify a client_id.
                    options.AcceptAnonymousClients();
                })
                .AddValidation();
        }
    }
}
