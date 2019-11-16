using CoreApp.Api.Middlewares;
using CoreApp.Domain.Entities;
using CoreApp.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;

namespace CoreApp.Api
{
    public class Startup
    {
        private const string primaryConnection = "PrimaryConnection";
        private const string appSettings = "AppSettings";
        private const string corsSettings = "CorsOrigin";

        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var corsOrigin = Configuration
                .GetSection(corsSettings)
                .GetChildren()
                .Select(s => s.Value)
                .ToArray();

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
            services.Configure<AppSettings>(Configuration.GetSection(appSettings));

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
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
                app.UseHsts(); // Available for 30 days
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors();
            //app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
