using System.Text.Json.Serialization;

namespace WeatherWatch.Infrastructure.Weather.Dtos;

internal sealed class DailyForecastDto
{
    [JsonPropertyName("dt")]
    public long Dt { get; init; }
    
    [JsonPropertyName("summary")]
    public string? Summary { get; init; }

    [JsonPropertyName("temp")]
    public TempDto Temp { get; init; } = new();
}