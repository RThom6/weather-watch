using WeatherWatch.Application.Weather;

namespace WeatherWatch.Application.Cities.Services;

public record CityDetails
{
    public Guid CityId { get; init; }
    public required string CountryCode { get; init; }
    public required string Name { get; init; }
    public required string State { get; init; }
    public required string Country { get; init; }
    public string? CurrencyCode { get; init; }
    public CurrentWeather CurrentWeather { get; init; }
}