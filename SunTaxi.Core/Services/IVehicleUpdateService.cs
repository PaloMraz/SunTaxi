using SunTaxi.Core.Data;

namespace SunTaxi.Core.Services
{
    /// <summary>
    /// Reprezentuje kontrakt pre aktualizácie záznamov o vozidlách v aplikačnej DB.
    /// </summary>
    public interface IVehicleUpdateService
  {
    /// <summary>
    /// Pridá alebo modifikuje záznamy o vozidlách na základe unikátneho <see cref="Vehicle.PlateNumber"/>.
    /// </summary>
    public Task CreateOrUpdateVehiclesAsync(IEnumerable<Vehicle> vehicles);
  }
}
