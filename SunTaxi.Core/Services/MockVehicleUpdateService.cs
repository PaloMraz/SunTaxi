﻿using SunTaxi.Core.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunTaxi.Core.Services
{
  /// <summary>
  /// Implementácia <see cref="IVehicleUpdateService"/> pre testovanie.
  /// </summary>
  public class MockVehicleUpdateService : IVehicleUpdateService
  {
    /// <summary>
    /// Implementácia len validuje <paramref name="vehicles"/> parameter a vypíše 
    /// informácie do Debug outputu.
    /// </summary>
    public Task CreateOrUpdateVehiclesAsync(IEnumerable<Vehicle> vehicles)
    {
      _ = vehicles ?? throw new ArgumentNullException(nameof(vehicles));
   
      foreach(var vehicle in vehicles)
      {
        Debug.WriteLine($"{vehicle}");
      }

      return Task.CompletedTask;
    }
  }
}
