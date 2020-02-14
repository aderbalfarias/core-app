using CoreApp.Api.Filters;
using CoreApp.Api.Middlewares;
using CoreApp.Api.Options.Authorization;
using CoreApp.Domain.Entities;
using CoreApp.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;

namespace CoreApp.Api
{
    public class Startup
    {
        private const string primaryConnection = "PrimaryConnection";
        private const string corsSettings = "CorsOrigin";
        private const string roleAdmin = "Admin";

        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Environment = environment;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var corsOrigin = Configuration
                .GetSection(corsSettings)
                .Get<string[]>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(corsOrigin)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            // DI
            services.Services();
            services.Repositories();
            services.Databases(Configuration.GetConnectionString(primaryConnection));
            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));

            var authenticationOption = Configuration
                .GetSection(nameof(ApplicationOptions.Authentication)).Get<AuthenticationOptions>();

            services.AddSingleton(authenticationOption);

            services.AddHealthChecks();
            //System.HealthCheckBuilderExtensions

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Core App",
                    Description = "Api to be template",
                    TermsOfService = new Uri("https://aderbalfarias.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "Aderbal Farias",
                        Email = "aderbalfarias@hotmail.com",
                        Url = new Uri("https://aderbalfarias.com")
                    }
                });

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Flows = new OpenApiOAuthFlows { }, //"application"
                    //OpenIdConnectUrl = new Uri(authenticationOption.Get<ApplicationOptions>().Authentication.TokenEndpoint)
                    OpenIdConnectUrl = new Uri("https://localhost:2222/")
                });

                c.OperationFilter<SwaggerAssignOAuth2SecurityFilter>();
            });

            //services.Configure<OIDCAuthorizationServerOptions>(
            //    Configuration.GetSection(nameof(ApplicationOptions.OIDCAuthorizationServer)));

            //var oidc = Configuration
            //    .GetSection(nameof(ApplicationOptions.OIDCAuthorizationServer))
            //    .Get<OIDCAuthorizationServerOptions>();

            //services.AddSingleton(oidc);
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(roleAdmin, policy => policy.RequireRole(roleAdmin));
            //});

            //services.OpenIddict();
            //services.OpenIdInitialize(Environment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _logger.LogInformation($"In {env.EnvironmentName} environment");

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

            //app.UseAuthentication();
            //app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors();
            app.UseRouting();
            app.UseHealthChecks("/ping");
            app.UseHealthChecks("/health", new HealthCheckOptions 
            { 
                //ResponseWriter = HealthChecks.UI.Client.UIResponseWriter.
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
