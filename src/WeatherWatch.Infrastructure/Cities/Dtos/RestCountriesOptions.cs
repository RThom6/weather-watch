namespace WeatherWatch.Infrastructure.Cities.Dtos;

public sealed record RestCountriesOptions
{
    public required string BaseUrl { get; init; }
    public required string ApiKey { get; init; }
}