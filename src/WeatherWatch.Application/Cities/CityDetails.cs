using WeatherWatch.Application.Weather;

namespace WeatherWatch.Application.Cities;

public record CityDetails
{
    public int CityId { get; init; }
    public required string CountryCode { get; init; }
    public required string Name { get; init; }
    public required string Country { get; init; }
    public string? CurrencyCode { get; init; }
    public int? EstimatedPopulation { get; init; }
    public decimal? TouristRating { get; init; }
    public DateOnly? DateEstablished { get; init; }
    public IReadOnlyList<DailyForecast> Forecast { get; init; }
}