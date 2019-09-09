using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CoreApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging(logging => 
            {
                logging.ClearProvides();
                logging.AddConsole();
                logging.AddDebug();
                logging.AddEventHandler();
            })
            .UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration)
            .UseStartup<Startup>();
    }
}
