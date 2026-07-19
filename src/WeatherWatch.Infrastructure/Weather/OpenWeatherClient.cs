using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using WeatherWatch.Application.Weather;
using WeatherWatch.Infrastructure.Weather.Dtos;

namespace WeatherWatch.Infrastructure.Weather;

internal sealed class OpenWeatherClient(HttpClient httpClient, IOptions<OpenWeatherOptions> options) : IWeatherService
{
    private readonly OpenWeatherOptions _options = options.Value;
    
    public async Task<WeatherInfo> GetCurrentWeatherByCoordinates(
        double latitude,
        double longitude,
        string? mode = "json",
        string? units = "metric",
        CancellationToken cancellationToken = default)
    {
        var uri = $"weather?lat={latitude}&lon={longitude}&mode={mode}&units={units}&appid={_options.ApiKey}";

        var response
            = await httpClient.GetFromJsonAsync<CurrentWeatherResponseDto>(uri, cancellationToken)
              ?? throw new InvalidOperationException("OpenWeather returned an empty response");

        return new WeatherInfo 
        {
            Summary = response.Weather?.FirstOrDefault()?.Description ?? "",
            Condition = response.Weather?.FirstOrDefault()?.Main ?? "",
            TemperatureCelsius = response.Main?.Temp ?? 0,
            FeelsLikeCelsius = response.Main?.FeelsLike ?? 0,
            Humidity = response.Main?.Humidity ?? 0,
            WindSpeed = response.Wind?.Speed ?? 0,
            ObservedAt = DateTimeOffset.FromUnixTimeSeconds(response.Dt),
        };
    }

    public async Task<IReadOnlyList<DailyForecast>> GetSixteenDayWeatherByCoordinates(
        double latitude,
        double longitude,
        string? mode = "json",
        string? units = "metric",
        CancellationToken cancellationToken = default)
    {
        var uri = $"forecast/daily?lat={latitude}&lon={longitude}&mode={mode}&units={units}&cnt=16&appid={_options.ApiKey}";

        var response = await httpClient.GetFromJsonAsync<DailyForecastResponseDto>(uri, cancellationToken)
                       ?? throw new InvalidOperationException("OpenWeather returned an empty response");

        return (response.List ?? [])
            .Select(ToDailyForecast)
            .ToList();
    }

    private static DailyForecast ToDailyForecast(DailyForecastItemDto item)
    {
        var weather = item.Weather?.FirstOrDefault();

        return new DailyForecast
        {
            Summary = weather?.Description ?? "",
            Condition = weather?.Main ?? "",
            WeatherId = weather?.Id ?? 0,
            Icon = weather?.Icon,
            ForecastedAt = DateTimeOffset.FromUnixTimeSeconds(item.Dt),
            Temperature = new DailyTemperature
            {
                Day = item.Temp?.Day ?? 0,
                Night = item.Temp?.Night ?? 0,
                Eve = item.Temp?.Eve ?? 0,
                Morn = item.Temp?.Morn ?? 0,
                Min = item.Temp?.Min,
                Max = item.Temp?.Max,
            },
            FeelsLike = new DailyTemperature
            {
                Day = item.FeelsLike?.Day ?? 0,
                Night = item.FeelsLike?.Night ?? 0,
                Eve = item.FeelsLike?.Eve ?? 0,
                Morn = item.FeelsLike?.Morn ?? 0,
            },
            PressureHpa = item.Pressure,
            Humidity = item.Humidity,
            WindSpeed = item.Speed,
            WindDirectionDegrees = item.Deg,
            WindGust = item.Gust,
            Cloudiness = item.Clouds,
            RainMm = item.Rain,
            SnowMm = item.Snow,
            PrecipitationChance = item.Pop,
        };
    }
}