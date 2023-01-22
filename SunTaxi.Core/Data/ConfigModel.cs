using Microsoft.Extensions.Configuration;
namespace SunTaxi.Core.Data
{
	public class ConfigModel
	{
		public ConfigModel(IConfiguration conf)
        {
           this.Path = conf["source:dataFilePath"];
           this.Encoding = conf["source:encoding"];
           this.NormalizeRegexPlateNumber= conf["normalizeRegexPlateNumber"];
           this.IdentRegexVehicleRecord = conf["identRegexVehicleRecord"];
        }

		public string? Path { get; private set; }
		public string? Encoding { get; private set; }
        public string? NormalizeRegexPlateNumber { get; private set; }
        public string? IdentRegexVehicleRecord { get; private set; }
    }
}

