using System;
using System.Collections.Generic;
using SunTaxi.Core.Data;

namespace SunTaxi.Core.Services
{
	public interface IConverterService
	{
        Task<IEnumerable<Vehicle>> processFileLines(IEnumerable<string> lines);

        IEnumerable<Vehicle> makeVehiclesDistinct(IEnumerable<Vehicle> vehicles);
    }
}

