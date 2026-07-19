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

    public async Task<IReadOnlyList<DailyForecast>> GetFiveDayForecastByCoordinates(
        double latitude,
        double longitude,
        string? mode = "json",
        string? units = "metric",
        CancellationToken cancellationToken = default)
    {
        var uri = $"forecast?lat={latitude}&lon={longitude}&mode={mode}&units={units}&appid={_options.ApiKey}";

        var response = await httpClient.GetFromJsonAsync<ForecastResponseDto>(uri, cancellationToken)
                       ?? throw new InvalidOperationException("OpenWeather returned an empty response");

        // The endpoint returns a reading every 3 hours, we just want one per day
        return (response.List ?? [])
            .GroupBy(entry => DateOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(entry.Dt).UtcDateTime))
            .OrderBy(day => day.Key)
            .Select(ToDailyForecast)
            .ToList();
    }

    private static DailyForecast ToDailyForecast(IGrouping<DateOnly, ForecastEntryDto> day)
    {
        // Use the reading nearest midday as the condition for the day.
        var midday = day
            .OrderBy(entry => Math.Abs(DateTimeOffset.FromUnixTimeSeconds(entry.Dt).UtcDateTime.Hour - 12))
            .First();

        var weather = midday.Weather?.FirstOrDefault();

        return new DailyForecast
        {
            Date = day.Key,
            Summary = weather?.Description ?? "",
            Condition = weather?.Main ?? "",
            Icon = weather?.Icon,
            MinCelsius = day.Min(entry => entry.Main?.TempMin ?? 0),
            MaxCelsius = day.Max(entry => entry.Main?.TempMax ?? 0),
            DayCelsius = midday.Main?.Temp ?? 0,
            Humidity = midday.Main?.Humidity ?? 0,
            WindSpeed = day.Max(entry => entry.Wind?.Speed ?? 0),
            PrecipitationChance = day.Max(entry => entry.Pop),
        };
    }
}