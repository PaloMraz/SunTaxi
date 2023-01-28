using SunTaxi.Core.Services.Interfaces;

namespace SunTaxi
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IVehicleUpdateService _vehicleUpdateService;
        private readonly IFileProcessingService _fileProcessingService;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public Worker(
            ILogger<Worker> logger,
            IVehicleUpdateService vehicleUpdateService,
            IFileProcessingService fileProcessingService,
            IHostApplicationLifetime hostApplicationLifetime
        )
        {
            _logger = logger;
            _vehicleUpdateService = vehicleUpdateService;
            _fileProcessingService = fileProcessingService;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Vehicle update started at: {DateTimeOffset.Now}");

            var fileLines = await _fileProcessingService.GetFileLinesAsync();

            if (fileLines is not null && fileLines.Count() > 1)
            {
                var vehicles = _fileProcessingService.ConvertFileLinesToVehicles(fileLines.Skip(1));

                _logger.LogInformation($"Upserting {vehicles.Count()} entries");
                await _vehicleUpdateService.CreateOrUpdateVehiclesAsync(vehicles);
            }
            else
            {
                _logger.LogInformation($"File contains no information");
            }

            _logger.LogInformation($"Vehicle update ended at: {DateTimeOffset.Now}");

            _hostApplicationLifetime.StopApplication();
        }
    }
}