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
            
        var lines = GetEcvLines(fileStream);
        var vehicles = lines
            .Where(IsEcvLine)
            .Select(LineToVehicle)
            .Where(_ => _ is not null); //TODO remove duplicates

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return await vehicles.ToArrayAsync();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
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
            Debug.WriteLine(fe);
            throw fe;
        }
    }

    private Vehicle? LineToVehicle(string line)
    {
        var values = line.Split("|");
        if (values.Length != 2)
            return null;

        var ecv = values.FirstOrDefault();
        if(ecv == null)
            return null;

        var normalized = _ecvNormalizer.NormalizeEcv(ecv);
        return new (){ PlateNumber=normalized, Name = values.LastOrDefault() };
    }
}
