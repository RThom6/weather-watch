using System.Text.Json.Serialization;
using WeatherWatch.Application.Weather;

namespace WeatherWatch.Infrastructure.Weather.Dtos;

internal sealed record OpenWeatherResponseDto
{
    [JsonPropertyName("daily")]
    public IReadOnlyList<DailyForecastDto> DailyForecasts { get; init; } = [];
}