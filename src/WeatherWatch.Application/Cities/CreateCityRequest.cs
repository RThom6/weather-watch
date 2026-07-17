namespace WeatherWatch.Application.Cities;

public record CreateCityRequest
{
    public string Name { get; init; } = "";
    public string State { get; init; } = "";
    public string Country { get; init; } = "";
    public string CountryCode { get; init; } = "";
}