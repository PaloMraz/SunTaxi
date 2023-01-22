using System.Text;
using Microsoft.Extensions.Logging;

namespace SunTaxi.Core.Services
{
    public class FileService : IFileService
    {

        #region Properties
        private const int _defaultBufferSize = 4096;
        private const FileOptions _defaultOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;
        private readonly ILogger<FileService> _logger;
        #endregion

        #region Constructors
        public FileService(ILogger<FileService> logger)
        {
            this._logger = logger;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Read file from configuration.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public async Task<string[]> ProcessReadAsync(string? filePath, Encoding? encoding = null)
        {

            try
            {
                if (!String.IsNullOrEmpty(filePath) && File.Exists(filePath) != false)
                {
                    //set default encoding if not specified
                    if (encoding == null)
                    {                        
                        encoding = this.detectEncoding(filePath);
                        this._logger.LogInformation("Auto detecting Encoding: {0}", encoding.EncodingName);
                    }
                    return await ReadFileAsync(filePath, encoding);
                }
                else
                {
                    throw new FileLoadException("The File was not found");
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Makes string array from file lines.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        async Task<string[]> ReadFileAsync(string filePath, Encoding encoding)
        {
            IList<string> lines = new List<string>();
            using (var sourceStream =
                new FileStream(
                    filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    _defaultBufferSize,
                    _defaultOptions))

            using (var reader = new StreamReader(sourceStream, encoding))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                }
            }
            return lines.ToArray();
        }

        /// <summary>
        /// Check full path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool isFullPath(string path)
        {
            try
            {
                return Path.GetFullPath(path) == path;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Detects Encoding, it should be improved in future.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private Encoding detectEncoding(string file)
        {
            using (var reader = new StreamReader(file, Encoding.Default, true))
            {
                if (reader.Peek() >= 0) // you need this!
                    reader.Read();

                return reader.CurrentEncoding;
            }
        }

        #endregion
    }
}

