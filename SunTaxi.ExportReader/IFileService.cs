namespace SunTaxi.ExportReader;

public interface IFileService
{
    StreamReader GetStream(string path);
}
