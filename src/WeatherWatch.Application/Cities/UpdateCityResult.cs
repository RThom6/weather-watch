namespace WeatherWatch.Application.Cities;

public record UpdateCityResult
{
    public required bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public int CityId { get; init; }
}