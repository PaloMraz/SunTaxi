using SunTaxi.Core.Services;

namespace SunTaxi;

public class SunTaxiService : ISunTaxiService
{
    private readonly IVehicleUpdateService _updateService;
    private readonly IExportReader _exportReader;

    public SunTaxiService(IVehicleUpdateService updateService, IExportReader exportReader)
    {
        _updateService = updateService;
        _exportReader = exportReader;
    }

    public async Task ImportFrom(string path)
    {
        var vehicles = await _exportReader.LoadVehicles(path);
        await _updateService.CreateOrUpdateVehiclesAsync(vehicles);
    }
}
