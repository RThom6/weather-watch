namespace WeatherWatch.Application.Weather;

public interface IWeatherService
{
    Task<WeatherInfo> GetCurrentWeatherByCoordinates(double latitude,
        double longitude,
        string? mode = "json",
        string? units = "metric",
        CancellationToken cancellationToken = default);
    
    Task<IReadOnlyList<DailyForecast>> GetFiveDayForecastByCoordinates(double latitude,
        double longitude,
        string? mode = "json",
        string? units = "metric",
        CancellationToken cancellationToken = default);
}