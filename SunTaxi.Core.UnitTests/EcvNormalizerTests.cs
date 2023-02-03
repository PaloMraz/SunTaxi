using SunTaxi.Core.Services;

namespace SunTaxi.Core.UnitTests;

public class EcvNormalizerTests
{   
    [Test]
    public void EcvNormalizer_NormalizeEmptyString_Success() =>
        Assert.IsEmpty(CreateNormalizer().NormalizeEcv(""));

    [Test]
    public void EcvNormalizer_NormalizeSampleData_Success() =>
        Assert.IsEmpty(CreateNormalizer().NormalizeEcv("XY-MB 921 "));

    private static IEcvNormalizer CreateNormalizer() => new EcvNormalizer();
}