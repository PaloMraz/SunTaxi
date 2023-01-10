using SunTaxi.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
