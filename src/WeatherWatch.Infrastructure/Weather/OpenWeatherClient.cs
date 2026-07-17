using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using WeatherWatch.Application.Weather;
using WeatherWatch.Infrastructure.Weather.Dtos;

namespace WeatherWatch.Infrastructure.Weather;

internal sealed class OpenWeatherClient(HttpClient httpClient, IOptions<OpenWeatherOptions> options) : IWeatherService
{
    private readonly OpenWeatherOptions _options = options.Value;
    
    public async Task<CurrentWeather> GetCurrentWeatherByCoordinates(
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

        return new CurrentWeather();
    }
}