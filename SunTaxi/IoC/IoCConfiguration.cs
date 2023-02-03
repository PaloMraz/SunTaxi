using Microsoft.Extensions.DependencyInjection;
using SunTaxi.Core.Services;
using SunTaxi.ExportReader;

namespace SunTaxi.IoC;

internal static class IoCConfiguration
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IVehicleUpdateService, MockVehicleUpdateService>();
        services.AddTransient<IEcvNormalizer, EcvNormalizer>();
        services.AddTransient<IExportReader, TxtExportReader>();
        services.AddTransient<ISunTaxiService, SunTaxiService>();
        services.AddTransient<IFileService, FileService>();

        return services;
    }
}
