using CoreApp.Api.Extensions;
using CoreApp.Api.Middlewares;
using CoreApp.Api.Options.Authorization;
using CoreApp.Domain.Entities;
using CoreApp.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;

namespace CoreApp.Api
{
    public class Startup
    {
        private const string DemoConnection = "DemoConnection";
        private const string corsSettings = "CorsOrigin";
        private const string roleAdmin = "Admin";

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. 
        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var corsOrigin = Configuration.GetSection(corsSettings).Get<string[]>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(corsOrigin).AllowAnyHeader().AllowAnyMethod();
                });
            });

            // Dependency Injection
            services.Services();
            services.Repositories();
            services.Databases(Configuration.GetConnectionString(DemoConnection));
            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));

            var authenticationOption = Configuration
                .GetSection(nameof(ApplicationOptions.Authentication))
                .Get<AuthenticationOptions>();

            services.AddSingleton(authenticationOption);

            services.AddHealthChecks()
                .AddSqlServer(Configuration.GetConnectionString(DemoConnection));

            services.AddControllers();
            services.AddApiVersioning();
            services.AddSwagger(authenticationOption);

            var oidc = Configuration
                .GetSection(nameof(ApplicationOptions.OidcAuthorizationServer))
                .Get<OidcAuthorizationServerOptions>();

            services.AddSingleton(oidc);
            services.AddAuthorization(options =>
            {
                options.AddPolicy(roleAdmin, policy => policy.RequireRole(roleAdmin));
            });

            services.AddOpenIddict(Environment, oidc, authenticationOption);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UseAuthentication();

            logger.LogInformation($"In {env.EnvironmentName} environment");

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Core App v1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMiddleware<ExceptionMiddleware>();
            }
            else
            {
                app.UseMiddleware<ExceptionMiddleware>();
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseHsts();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors();
            app.UseHealthChecks();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            OpenIddictExtension.OpenIdInitializeAsync(app.ApplicationServices).GetAwaiter().GetResult();
        }
    }
}
