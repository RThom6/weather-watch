using System.Text.Json.Serialization;

namespace WeatherWatch.Infrastructure.Weather.Dtos;

/// <summary>
/// Dto for OpenWeatherApi's 5 day / 3 hour forecast Api
/// https://openweathermap.org/api/forecast5?collection=current_forecast
/// </summary>
internal sealed record ForecastResponseDto
{
    [JsonPropertyName("list")]
    public List<ForecastEntryDto>? List { get; init; }
}

internal sealed record ForecastEntryDto
{
    [JsonPropertyName("dt")]
    public long Dt { get; init; }

    [JsonPropertyName("main")]
    public MainDto? Main { get; init; }

    [JsonPropertyName("weather")]
    public List<WeatherDto>? Weather { get; init; }

    [JsonPropertyName("clouds")]
    public CloudsDto? Clouds { get; init; }

    [JsonPropertyName("wind")]
    public WindDto? Wind { get; init; }

    [JsonPropertyName("visibility")]
    public int Visibility { get; init; }

    [JsonPropertyName("pop")]
    public double Pop { get; init; } // probability of precipitation

    [JsonPropertyName("rain")]
    public ForecastPrecipitationDto? Rain { get; init; }

    [JsonPropertyName("snow")]
    public ForecastPrecipitationDto? Snow { get; init; }

    [JsonPropertyName("dt_txt")]
    public string? DtTxt { get; init; }
}

internal sealed record ForecastPrecipitationDto
{
    [JsonPropertyName("3h")]
    public double? ThreeHour { get; init; }
}
