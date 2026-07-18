namespace WeatherWatch.Application.Cities;

public record DeleteCityResult
{
    public required bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
}