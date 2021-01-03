using CoreApp.Api.Extensions;
using CoreApp.Api.Middlewares;
using CoreApp.Api.Options.Authorization;
using CoreApp.Domain.Entities;
using CoreApp.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreApp.Api
{
    public class Startup
    {
        private const string DemoConnection = "DemoConnection";
        private const string corsSettings = "CorsOrigin";
        private const string roleAdmin = "Admin";
        private string[] Schemes = { JwtBearerDefaults.AuthenticationScheme, "ADFS" };

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
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(Schemes)
                    .Build();
            });

            services.AddBearers(Environment, oidc, authenticationOption, Schemes);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("Server", "DENY");

                await next();
            });

            app.UseAuthentication();

            logger.LogInformation($"In {env.EnvironmentName} environment");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v3/swagger.json", "Core App .NET v5");
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

                app.UseHttpsRedirection();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseCors();
            app.UseHealthChecks();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            BearersExtension.OpenIdInitializeAsync(app.ApplicationServices).GetAwaiter().GetResult();
        }
    }
}
