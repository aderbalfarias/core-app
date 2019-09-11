using CoreApp.Data.Config;
using CoreApp.Data.Mappings;
using CoreApp.Data.Repositories;
using CoreApp.Domain.Interfaces.Repositories;
using CoreApp.Domain.Interfaces.Services;
using CoreApp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoreApp.IoC
{
    public static class RegisterDependencies
    {
        public static IServiceCollection Services(this IServiceCollection services)
        {
            services.AddScoped<ITestService, TestService>();

            return services;
        }

        public static IServiceCollection Repositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository, BaseRepository>();

            return services;
        }

        public static IServiceCollection Databases(this IServiceCollection services, string connectionString)
        {
            // Add configuration for DbContext
            // Use connection string from appsettings.json file
            services.AddDbContext<PrimaryContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<ILogger, Logger<PrimaryContext>>();

            return services;
        }
    }
}
