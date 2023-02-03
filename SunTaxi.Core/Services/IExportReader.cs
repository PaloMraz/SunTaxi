
using SunTaxi.Core.Data;

namespace SunTaxi.Core.Services;

public interface IExportReader
{
    Task<IEnumerable<Vehicle>> LoadVehicles(string path);
}
