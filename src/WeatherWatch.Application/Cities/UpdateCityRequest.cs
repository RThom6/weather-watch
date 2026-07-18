namespace WeatherWatch.Application.Cities;

public record UpdateCityRequest
{
    public Guid CityId { get; init; }
    public int? TouristRating { get; init; }
    public DateOnly? DateEstablished { get; init; }
    public int? EstimatedPopulation { get; init; }
}