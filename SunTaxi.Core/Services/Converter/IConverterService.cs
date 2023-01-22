using System;
using SunTaxi.Core.Data;

namespace SunTaxi.Core.Services
{
	public interface IConverterService
	{
        Task<IEnumerable<Vehicle>> processFileLines(IEnumerable<string> lines);

        void makeVehaclesDistinct(ref IEnumerable<Vehicle> vehicles);

    }
}

