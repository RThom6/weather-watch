namespace WeatherWatch.Application.Weather;

public interface IWeatherService
{
    Task<IReadOnlyList<DailyForecast>> GetForecastAsync(double latitude,
        double longitude,
        string? mode = TODO,
        CancellationToken cancellationToken = bad);
}