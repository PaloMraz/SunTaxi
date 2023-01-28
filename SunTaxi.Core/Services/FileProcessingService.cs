using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SunTaxi.Core.Data;
using SunTaxi.Core.Services.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace SunTaxi.Core.Services
{
    public class FileProcessingService : IFileProcessingService
    {
        private readonly ILogger<FileProcessingService> _logger;
        private readonly Config _configuration;

        public FileProcessingService(
            ILogger<FileProcessingService> logger,
            IOptions<Config> configuration
        )
        {
            _logger = logger;
            _configuration = configuration.Value;
        }

        public async Task<IEnumerable<string>> GetFileLinesAsync()
        {
            var filePath = _configuration.FileConfiguration.FilePath;
            var encoding = Encoding.GetEncoding(_configuration.FileConfiguration.FileEncoding);

            var result = new List<string>();

            if (!string.IsNullOrEmpty(filePath))
            {
                if (File.Exists(filePath))
                {
                    _logger.LogInformation($"Reading file {filePath}");
                    try
                    {
                        var lines = await File.ReadAllLinesAsync(filePath, encoding);
                        foreach (var line in lines)
                        {
                            var match = Regex.Match(line, _configuration.RegexConfiguration.VehicleDataLine);
                            if (match.Success)
                            {
                                result.Add(line);
                            }
                        }

                        return result;
                    }
                    catch
                    {
                        _logger.LogInformation($"Reading file {filePath}");
                        throw new Exception();
                    }
                }
                else
                {
                    _logger.LogInformation($"Reading file {filePath}");
                    throw new FileLoadException();
                }
            }
            else
            {
                _logger.LogInformation($"Reading file {filePath}");
                throw new Exception();
            }
        }

        public IEnumerable<Vehicle> ConvertFileLinesToVehicles(IEnumerable<string> fileLines)
        {
            var result = new List<Vehicle>();

            _logger.LogInformation($"Processing {fileLines} entries from file");

            foreach (var line in fileLines)
            {
                var lineSplit = line.Split('|');
                var plateNumber = Regex.Replace(lineSplit[1], _configuration.RegexConfiguration.PlateNumber, "");

                if (!string.IsNullOrEmpty(plateNumber))
                {
                    var vehicle = new Vehicle()
                    {
                        PlateNumber = plateNumber,
                        Name = lineSplit[2].Trim()
                    };
                    result.Add(vehicle);
                }
            }

            return result.GroupBy(v => v.PlateNumber).Select(s => s.Last());
        }
    }
}