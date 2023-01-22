using System.Text.RegularExpressions;
using SunTaxi.Core.Data;

namespace SunTaxi.Core.Services
{
    public class ConverterService: IConverterService
    {
        #region Properties
        const string patternLine = @"[^|]*";
        private readonly ConfigModel _config;
        #endregion

        #region Public constructor
        public ConverterService(ConfigModel config)
        {
            this._config = config;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Main method to to convert file line to Vehicle record
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public  Task<IEnumerable<Vehicle>> processFileLines(IEnumerable<string> lines)
        {
           var result = lines
                //try to convert the line to 2 string columns (plateNumber and name)
                .Select(line => this.convertLineToColumns(line))
                //ignore incorrect lines when converting failed or when plateNumber is blank
                .Where(columns => columns != null)
                //skip header becuse the identRegexVehicleRecord matches header record
                .Skip(1)
                // normalized text
                .Select(columns => this.normalizeColumns(columns))
                // make Vehicle record
                .Select(columns => this.makeVehicleRecord(columns));
              
           return Task.FromResult<IEnumerable<Vehicle>>(result);
        }

        /// <summary>
        /// Remove duplicate records by PlateNumber keys and takes last existing one
        /// </summary>
        /// <param name="vehicles"></param>
        /// <returns></returns>
        public IEnumerable<Vehicle> makeVehiclesDistinct(IEnumerable<Vehicle> vehicles)
        {
            return vehicles.GroupBy(gb => gb.PlateNumber).Select(s => s.Last()).ToList();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Test if the file line matches criteria for vehicle row
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string[]? convertLineToColumns(string line)
        {
            if (!string.IsNullOrEmpty(this._config.IdentRegexVehicleRecord))
            {
                var matches= Regex.Matches(line, this._config.IdentRegexVehicleRecord);
                if (matches.Any()) {
                    return new string[2] { matches[0].Groups[1].Value, matches[0].Groups[2].Value };
                }
            }
            return null;

        }
        /// <summary>
        /// Convert string array to Vehicle record
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private Vehicle makeVehicleRecord(string[]? columns)
        {
            if (columns != null && columns.Length==2)
            {               
                return new Vehicle() { PlateNumber = columns[0], Name = columns[1] };
            }
            else throw new Exception("Cannot create Vehicle record");
        }

        /// <summary>
        /// Normalize PlateNumber
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        private string[]? normalizeColumns(string[]? columns)
        {
            if (!string.IsNullOrEmpty(this._config.NormalizeRegexPlateNumber) && columns != null) {
                //normalise plateNumber
                columns[0] = Regex.Replace(columns[0], this._config.NormalizeRegexPlateNumber, @"");
            }          
            return columns;
        }
        #endregion
    }
}

