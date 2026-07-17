namespace WeatherWatch.Application.Cities;

public record CreateCityResult
{
    public required bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public Guid CityId { get; init; }
}