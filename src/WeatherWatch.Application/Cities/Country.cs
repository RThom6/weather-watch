namespace WeatherWatch.Application.Cities;

public record Country
{
    public string IsoCode { get; init; }
    public string Name { get; init; }
    public string? CurrencyCode { get; init; }
    public IReadOnlyList<Capital> Capitals { get; init; } = [];
}