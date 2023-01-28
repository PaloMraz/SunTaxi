using SunTaxi.Core.Data;

namespace SunTaxi.Core.Services.Interfaces
{
    public interface IFileProcessingService
    {
        Task<IEnumerable<string>> GetFileLinesAsync();

        IEnumerable<Vehicle> ConvertFileLinesToVehicles(IEnumerable<string> fileLines);
    }
}