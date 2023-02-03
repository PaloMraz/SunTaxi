using System.Diagnostics;

namespace SunTaxi.ExportReader;

public class FileService : IFileService
{
    public StreamReader GetStream(string path)
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
}
