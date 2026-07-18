namespace WeatherWatch.Application.Cities;

public record CitySummary
{
    public required Guid CityId { get; init; }
    public required string Name { get; init; }
    public required string CountryName { get; init; }
}