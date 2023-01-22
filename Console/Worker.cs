using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SunTaxi.Core.Data;
using SunTaxi.Core.Services;

namespace Console
{
    public class Worker : IHostedService
    {
        private readonly IFileService _fileService;
        private readonly IConverterService _converterService;
        private readonly ConfigModel _config;
        private readonly IVehicleUpdateService _db;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ILogger<Worker> _logger;
        private int _exitCode = 0;

        public Worker(IFileService fileService,
            IConverterService converterService,
            ConfigModel config,
            IVehicleUpdateService db,
            IHostApplicationLifetime appLifetime,
            ILogger<Worker> logger)
        {
            this._converterService = converterService;
            this._fileService = fileService;
            this._config = config;
            this._db = db;
            this._appLifetime = appLifetime;
            this._logger = logger;        
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                this._logger.LogInformation("Start Program");
                // get lines from file
                string[] lines = this._fileService.ProcessReadAsync(this._config.Path, Encoding.Default).Result;             
                this._logger.LogInformation("Total file lines: {0}", lines.Length);

                //convert lines to vehacle records
                var vehacles = _converterService.processFileLines(lines).Result;
                this._logger.LogInformation("Vehacle records: {0}", vehacles.Count());

                // even ref is not needed bcs reference type
                this._converterService.makeVehaclesDistinct(ref vehacles);
                this._logger.LogInformation("Vehacle distinct records: {0}", vehacles.Count());

                //populate DB
                this._db.CreateOrUpdateVehiclesAsync(vehacles);
                this._logger.LogInformation("Mock Database updated");

            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "An error occurs");
                this._exitCode = 1;
            }

            finally
            {
                // Stop the application once the work is done
                      this._appLifetime.StopApplication();               
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Environment.ExitCode = this._exitCode;
            this._logger.LogInformation("Finish program with exit code: {0}", this._exitCode);
            return Task.CompletedTask;
        }

    }
}

