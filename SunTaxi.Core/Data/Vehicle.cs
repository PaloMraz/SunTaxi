namespace SunTaxi.Core.Data
{
    /// <summary>
    /// Entita reprezentujúca záznam o vozidle čo sa importuje zo SAPu.
    /// </summary>
    public record Vehicle
  {
    /// <summary>
    /// EČV 
    /// </summary>
    public required string PlateNumber { get; init; }

    /// <summary>
    /// Názov typu vozidla napr.: VW-Golf
    /// </summary>
    public string? Name { get; init; }
  }
}
