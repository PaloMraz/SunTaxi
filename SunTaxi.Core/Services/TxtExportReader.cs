using System.Diagnostics;
using SunTaxi.Core.Data;

namespace SunTaxi.Core.Services;

public sealed class TxtExportReader : IExportReader
{
    private const string VehicleLineStart = "|";
    private const string HeaderLineStart = "|KFZKZ";

    private readonly IEcvNormalizer _ecvNormalizer;

    public TxtExportReader(IEcvNormalizer ecvNormalizer)
    {
        _ecvNormalizer = ecvNormalizer;
    }

    public async Task<IEnumerable<Vehicle>> LoadVehicles(string path)
    {
        using var fileStream = GetStream(path);

        var vehicles = await GetEcvLines(fileStream)
            .Where(IsEcvLine)
            .Select(LineToVehicle)
            .Where(_ => _ is not null)
            .Select(_ => _!)
            .ToArrayAsync();

        var uniqueVehicles = vehicles
            .GroupBy(_ => _.PlateNumber)
            .Select(_ => _.Last());

        return uniqueVehicles;
    }

    private async IAsyncEnumerable<string> GetEcvLines(StreamReader streamReader)
    {
        string? line;
        while ((line = await streamReader.ReadLineAsync()) is not null)
        {
            yield return line;
        }
    }

    private static bool IsEcvLine(string? line) =>
        line is not null
        && line.StartsWith(VehicleLineStart)
        && !line.StartsWith(HeaderLineStart);

    private StreamReader GetStream(string path)
    {
        try
        {
            return File.OpenText(path);
        }
        catch (FileNotFoundException fe)
        {
            Console.WriteLine($"Failed to open file '{path}'");
            Debug.WriteLine(fe.StackTrace);
            throw fe;
        }
    }

    private Vehicle? LineToVehicle(string line)
    {
        var values = line.Trim('|').Split("|");
        if (values.Length != 2 || string.IsNullOrWhiteSpace(values.First()))
            return null;

        var ecv = values.FirstOrDefault();
        if(ecv == null)
            return null;

        return new (){ PlateNumber=NormalizeEcv(ecv), Name = values.LastOrDefault() };
    }

    private string NormalizeEcv(string ecv) => _ecvNormalizer.NormalizeEcv(ecv);
}
