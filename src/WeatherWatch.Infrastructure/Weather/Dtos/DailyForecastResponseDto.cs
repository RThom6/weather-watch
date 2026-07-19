using System.Text.Json.Serialization;

namespace WeatherWatch.Infrastructure.Weather.Dtos;

/// <summary>
/// Dto includes all of the fields potentially returned by OpenWeatherApi's forecast Api:
/// https://openweathermap.org/api/current?collection=current_forecast
/// </summary>
internal sealed record DailyForecastResponseDto
{
    [JsonPropertyName("list")]
    public List<DailyForecastItemDto>? List { get; init; }
}

internal sealed record DailyForecastItemDto
{
    [JsonPropertyName("dt")]
    public long Dt { get; init; }

    [JsonPropertyName("temp")]
    public DailyTempDto? Temp { get; init; }

    [JsonPropertyName("feels_like")]
    public DailyFeelsLikeDto? FeelsLike { get; init; }

    [JsonPropertyName("pressure")]
    public int Pressure { get; init; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; init; }

    [JsonPropertyName("weather")]
    public List<WeatherDto>? Weather { get; init; }

    [JsonPropertyName("speed")]
    public double Speed { get; init; }

    [JsonPropertyName("deg")]
    public int Deg { get; init; }

    [JsonPropertyName("gust")]
    public double? Gust { get; init; }

    [JsonPropertyName("clouds")]
    public int Clouds { get; init; }

    [JsonPropertyName("rain")]
    public double? Rain { get; init; }

    [JsonPropertyName("snow")]
    public double? Snow { get; init; }

    [JsonPropertyName("pop")]
    public double Pop { get; init; }
}

internal sealed record DailyTempDto
{
    [JsonPropertyName("day")]
    public double Day { get; init; }

    [JsonPropertyName("min")]
    public double Min { get; init; }

    [JsonPropertyName("max")]
    public double Max { get; init; }

    [JsonPropertyName("night")]
    public double Night { get; init; }

    [JsonPropertyName("eve")]
    public double Eve { get; init; }

    [JsonPropertyName("morn")]
    public double Morn { get; init; }
}

internal sealed record DailyFeelsLikeDto
{
    [JsonPropertyName("day")]
    public double Day { get; init; }

    [JsonPropertyName("night")]
    public double Night { get; init; }

    [JsonPropertyName("eve")]
    public double Eve { get; init; }

    [JsonPropertyName("morn")]
    public double Morn { get; init; }
}
