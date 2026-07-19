using WeatherWatch.Application.Weather;

namespace WeatherWatch.Application.Cities;

public record CityDetailsPreview
{
    public int CityId { get; init; }
    public required string Name { get; init; }
    public required string Country { get; init; }
    public required WeatherInfo CurrentWeather { get; init; }
}