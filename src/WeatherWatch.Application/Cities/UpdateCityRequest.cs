namespace WeatherWatch.Application.Cities;

public record UpdateCityRequest
{
    public decimal? TouristRating { get; init; }
    public DateOnly? DateEstablished { get; init; }
    public int? EstimatedPopulation { get; init; }
}