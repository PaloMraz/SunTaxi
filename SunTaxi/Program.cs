using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SunTaxi.Core.Services;
using SunTaxi.IoC;
using SunTaxi.Settings;

var configuration = new ConfigurationBuilder()
     .AddJsonFile($"appsettings.json")
     .Build();

var serviceProvider = new ServiceCollection()
    .RegisterServices()
    .BuildServiceProvider();

try
{
    var paths = configuration.GetRequiredSection(nameof(Paths)).Get<Paths>()
        ?? throw new Exception("Can not retrieve Path from appsettings.json");
    var sunTaxiService = serviceProvider.GetService<ISunTaxiService>()
        ?? throw new Exception($"Service not registered in container");
    await sunTaxiService.ImportFrom(paths.SAPFile);
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
    Debug.WriteLine(ex.StackTrace);
    return 1;
}

return 0;