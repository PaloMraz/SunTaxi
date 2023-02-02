using System.Text.RegularExpressions;

namespace SunTaxi.Core.Services;

public sealed class EcvNormalizer : IEcvNormalizer
{
    private static readonly Regex Filter = new("[^a-zA-Z0-9]");

    public string NormalizeEcv(string ecv) => EcvIsEmpty(ecv) ? string.Empty : TrimEcv(ecv);

    private string TrimEcv(string ecv) => Filter.Replace(ecv, "");

    private bool EcvIsEmpty(string ecv) => string.IsNullOrWhiteSpace(ecv);
}

