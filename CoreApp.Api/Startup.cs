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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Core App Template",
                    Description = "Project developed to be used as a template for new .net core apis",
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
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            //AuthorizationUrl = new Uri("/auth-server/connect/authorize", UriKind.Relative),
                            //TokenUrl = new Uri("/auth-server/connect/token", UriKind.Relative)
                            //TokenUrl = new Uri(authenticationOptions.TokenEndpoint),
                            TokenUrl = new Uri(authenticationOption.TokenEndpoint),
                            Scopes = new Dictionary<string, string>
                            {
                                //{ "readAccess", "Access read operations" },
                                //{ "writeAccess", "Access write operations" }
                            }
                        }
                    }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                        },
                        new string[] { }
                    }
                });
            });

            var oidc = Configuration
                .GetSection(nameof(ApplicationOptions.OIDCAuthorizationServer))
                .Get<OIDCAuthorizationServerOptions>();

            services.AddSingleton(oidc);
            services.AddAuthorization(options =>
            {
                options.AddPolicy(roleAdmin, policy => policy.RequireRole(roleAdmin));
            });

            services.AddOpenIddict(Environment);
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
            app.UseHealthChecks("/ping");
            app.UseHealthChecks("/health", new HealthCheckOptions
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            OpenIddictExtension.OpenIdInitializeAsync(app.ApplicationServices).GetAwaiter().GetResult();
        }
    }
}
