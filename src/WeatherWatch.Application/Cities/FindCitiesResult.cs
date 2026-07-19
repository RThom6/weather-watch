namespace WeatherWatch.Application.Cities;

public record FindCitiesResult
{
    public IReadOnlyList<SimpleCityInfo> Cities { get; init; } = [];
}

public record SimpleCityInfo
{
    public string Name { get; init; }
    public string Country { get; init; }
    public string State { get; init; }
    public string CountryCode { get; init; }
}