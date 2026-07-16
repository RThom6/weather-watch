using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using WeatherWatch.Application.Weather;
using WeatherWatch.Infrastructure.Weather.Dtos;

namespace WeatherWatch.Infrastructure.Weather;

internal sealed class OpenWeatherClient(HttpClient httpClient, IOptions<OpenWeatherOptions> options) : IWeatherService
{
    private readonly OpenWeatherOptions _options = options.Value;
    
    public async Task<IReadOnlyList<DailyForecast>> GetForecastAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken = default)
    {
        var url = $"onecall?lat={latitude}&lon={longitude}" 
                  + "&units=metric&exclude=current,minutely,hourly,alerts" 
                  + $"&appid={_options.ApiKey}";;

        var response
            = await httpClient.GetFromJsonAsync<OpenWeatherResponseDto>(url, cancellationToken)
              ?? throw new InvalidOperationException("OpenWeather returned an empty response");
        
        return response.DailyForecasts
            .Select(d => new DailyForecast
            {
                Date = DateOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(d.Dt).UtcDateTime),
                Temperature = new Temp { Celsius = d.Temp.Day },
                Summary = d.Summary,
            }).ToList();
    }
}