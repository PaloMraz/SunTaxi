using Microsoft.Extensions.Configuration;
using System.Text;
namespace SunTaxi.Core.Data
{
    public class ConfigModel
    {
        public ConfigModel(IConfiguration conf)
        {
            this.Path = conf["source:dataFilePath"];
            this.Encoding = conf["source:encoding"];
            this.NormalizeRegexPlateNumber = conf["normalizeRegexPlateNumber"];
            this.IdentRegexVehicleRecord = conf["identRegexVehicleRecord"];
        }

        public string? Path { get; private set; }
        public string? Encoding { get; private set; }
        public string? NormalizeRegexPlateNumber { get; private set; }
        public string? IdentRegexVehicleRecord { get; private set; }

        public Encoding? EncodingFromConfing
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Encoding))
                {
                    var item = System.Text.Encoding.GetEncodings().FirstOrDefault(r => r.Name == this.Encoding);
                    if (item != null)
                    {
                        return System.Text.Encoding.GetEncoding(item.CodePage);
                    }
                }
                return null;
            }
        }
    }
}

