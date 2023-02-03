using System.Text;
using Moq;
using SunTaxi.Core.Services;

namespace SunTaxi.ExportReader.UnitTests;

public class TxtExportReaderTests
{
    private const string SamplePath = nameof(SamplePath);

    [Test]
    public async Task TxtExportReader_LoadVehiclesEmptyString_Success()
    {
        const string text = "";
        var normalizerMock = CreateNormalizerMock();
        normalizerMock.Setup(_ => _.NormalizeEcv(It.Is<string>(_ => _ == string.Empty))).Returns(string.Empty);
        var fileServiceMock = CreateFileServiceMock();
        fileServiceMock.Setup(_ => _.GetStream(It.IsAny<string>())).Returns(AsStreamReader(text));

        var exportReader = CreateTxtExportReader(normalizerMock.Object, fileServiceMock.Object);
        var result = await exportReader.LoadVehicles(SamplePath);
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task TxtExportReader_LoadVehicles_Success()
    {
        const string ecv = "SK123SK";
        const string name = "name";
        const string text = $@"----
|{ecv}|{name}|
Some invalid line";
        var normalizerMock = CreateNormalizerMock();
        normalizerMock.Setup(_ => _.NormalizeEcv(It.IsAny<string>())).Returns(ecv);
        var fileServiceMock = CreateFileServiceMock();
        fileServiceMock.Setup(_ => _.GetStream(It.IsAny<string>())).Returns(AsStreamReader(text));

        var exportReader = CreateTxtExportReader(normalizerMock.Object, fileServiceMock.Object);
        var result = await exportReader.LoadVehicles(SamplePath);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));

        var vehicle = result.First();
        Assert.Multiple(() =>
        {
            Assert.That(vehicle.PlateNumber, Is.EqualTo(ecv));
            Assert.That(vehicle.Name, Is.EqualTo(name));
        });
    }

    [Test]
    public void TxtExportReader_LoadVehiclesFileNotFound_Throws()
    {
        var normalizerMock = CreateNormalizerMock();
        var fileServiceMock = CreateFileServiceMock();
        fileServiceMock.Setup(_ => _.GetStream(It.IsAny<string>())).Throws<FileNotFoundException>();
        var exportReader = CreateTxtExportReader(normalizerMock.Object, fileServiceMock.Object);
        Assert.ThrowsAsync<FileNotFoundException>(async () => await exportReader.LoadVehicles(SamplePath));
    }

    private StreamReader AsStreamReader(string text) => new(AsMemoryStream(text));

    private Stream AsMemoryStream(string text) => new MemoryStream(Encoding.UTF8.GetBytes(text ?? string.Empty));

    private static Mock<IEcvNormalizer> CreateNormalizerMock() => new Mock<IEcvNormalizer>();

    private static Mock<IFileService> CreateFileServiceMock() => new Mock<IFileService>();

    private static IExportReader CreateTxtExportReader(IEcvNormalizer normalizer, IFileService fileService) =>
        new TxtExportReader(normalizer, fileService);
}