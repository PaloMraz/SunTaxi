using Microsoft.Extensions.DependencyInjection;
using SunTaxi.IoC;

var serviceProvider = new ServiceCollection()
    .RegisterServices()
    .BuildServiceProvider();


Console.WriteLine("Hi from SunTaxi");
Console.Read();