using SunTaxi.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Console;
using SunTaxi.Core.Data;


namespace ConsoleApp
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Environment.ExitCode = 0;
            try
            {
                CreateHostBuilder(args).Build().RunAsync();
            }
            catch {
                Environment.ExitCode = 1;
            }
            return Environment.ExitCode;
        }

        static IHostBuilder CreateHostBuilder(string[] args)
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
                 .ConfigureServices((_, services) => services
                             .AddSingleton<IFileService, FileService>()
                             .AddTransient<IVehicleUpdateService, MockVehicleUpdateService>()
                             .AddSingleton<IConverterService, ConverterService>()
                             .AddHostedService<Worker>()
                             .AddSingleton<ConfigModel>());
        }
    }
}

