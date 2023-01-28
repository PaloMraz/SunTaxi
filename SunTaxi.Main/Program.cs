using SunTaxi.Core.Data;
using SunTaxi.Core.Services;
using SunTaxi.Core.Services.Interfaces;

namespace SunTaxi
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            try
            {
                await host.RunAsync();
            }
            catch
            {
                Environment.Exit(1);
            }

            Environment.Exit(0);
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false);
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddTransient<IVehicleUpdateService, MockVehicleUpdateService>()
                        .AddSingleton<IFileProcessingService, FileProcessingService>()
                        .AddHostedService<Worker>();

                    services.Configure<Config>(hostContext.Configuration.GetSection("Config"));
                });
        }
    }
}