namespace SunTaxi.Core.Data
{
    public class Config
    {
        public FileConfiguration FileConfiguration { get; set; }

        public RegexConfiguration RegexConfiguration { get; set; }
    }

    public class FileConfiguration
    {
        public string FilePath { get; set; }
        public string FileEncoding { get; set; }
    }

    public class RegexConfiguration
    {
        public string VehicleDataLine { get; set; }
        public string PlateNumber { get; set; }
    }
}