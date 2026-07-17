using System.Text.Json.Serialization;

namespace WeatherWatch.Infrastructure.Weather.Dtos;

/// <summary>
/// Dto includes all of the fields potentially returned by OpenWeatherApi's current forecast Api:
/// https://openweathermap.org/api/current?collection=current_forecast
/// </summary>
internal sealed record CurrentWeatherResponseDto
{
    [JsonPropertyName("coord")]
    public CoordDto? Coord { get; init; }

    [JsonPropertyName("weather")]
    public List<WeatherDto>? Weather { get; init; }

    [JsonPropertyName("base")]
    public string? Base { get; init; }

    [JsonPropertyName("main")]
    public MainDto? Main { get; init; }

    [JsonPropertyName("visibility")]
    public int Visibility { get; init; }

    [JsonPropertyName("wind")]
    public WindDto? Wind { get; init; }

    [JsonPropertyName("clouds")]
    public CloudsDto? Clouds { get; init; }

    [JsonPropertyName("rain")]
    public RainDto? Rain { get; init; }
    
    [JsonPropertyName("snow")]
    public SnowDto? Snow { get; init; }

    [JsonPropertyName("dt")]
    public long Dt { get; init; }

    [JsonPropertyName("sys")]
    public SysDto? Sys { get; init; }

    [JsonPropertyName("timezone")]
    public int UtcOffsetSec { get; init; } // Timezone shift from Utc in seconds

    [JsonPropertyName("id")]
    public int CityId { get; init; }

    [JsonPropertyName("name")]
    public string? City { get; init; }

    [JsonPropertyName("cod")]
    public int Cod { get; init; }
}

internal sealed record CoordDto
{
    [JsonPropertyName("lon")]
    public double Longitude { get; init; }

    [JsonPropertyName("lat")]
    public double Latitude { get; init; }
}

internal sealed record WeatherDto
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("main")]
    public string? Main { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("icon")]
    public string? Icon { get; init; }
}

internal sealed record MainDto
{
    [JsonPropertyName("temp")]
    public double Temp { get; init; }

    [JsonPropertyName("feels_like")]
    public double FeelsLike { get; init; }

    [JsonPropertyName("temp_min")]
    public double TempMin { get; init; }

    [JsonPropertyName("temp_max")]
    public double TempMax { get; init; }

    [JsonPropertyName("pressure")]
    public int Pressure { get; init; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; init; }

    [JsonPropertyName("sea_level")]
    public int? SeaLevel { get; init; }

    [JsonPropertyName("grnd_level")]
    public int? GroundLevel { get; init; }
}

internal sealed record WindDto
{
    [JsonPropertyName("speed")]
    public double Speed { get; init; }

    [JsonPropertyName("deg")]
    public int Deg { get; init; }

    [JsonPropertyName("gust")]
    public double? Gust { get; init; }
}

internal sealed record RainDto
{
    [JsonPropertyName("1h")]
    public double? OneHour { get; init; }
}

internal sealed record SnowDto
{
    [JsonPropertyName("1h")]
    public double? OneHour { get; init; }
}

internal sealed record CloudsDto
{
    [JsonPropertyName("all")]
    public int All { get; init; }
}

internal sealed record SysDto
{
    [JsonPropertyName("type")]
    public int? Type { get; init; }

    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonPropertyName("country")]
    public string? Country { get; init; }

    [JsonPropertyName("sunrise")]
    public long Sunrise { get; init; }

    [JsonPropertyName("sunset")]
    public long Sunset { get; init; }
}