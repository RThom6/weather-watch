using System.Text.Json.Serialization;

namespace WeatherWatch.Infrastructure.Weather.Dtos;

public class TempDto
{
    [JsonPropertyName("day")]
    public decimal Day { get; init; }
}