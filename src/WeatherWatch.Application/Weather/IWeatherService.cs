namespace WeatherWatch.Application.Weather;

public interface IWeatherService
{
    Task<IReadOnlyList<DailyForecast>> GetCurrentWeatherAsync(double latitude,
        double longitude,
        string? mode = "json",
        string? units = "metric",
        CancellationToken cancellationToken = default);
}