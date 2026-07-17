namespace WeatherWatch.Application.Weather;

public interface IWeatherService
{
    Task<CurrentWeather> GetCurrentWeatherByCoordinates(double latitude,
        double longitude,
        string? mode = "json",
        string? units = "metric",
        CancellationToken cancellationToken = default);
}