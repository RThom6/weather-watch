using System.Text.Json.Serialization;

namespace WeatherWatch.Infrastructure.Cities.Dtos;

public record CountryResponseDto
{
    [JsonPropertyName("data")]
    public CountryDataDto? Data { get; init; }
}
  
public record CountryDataDto
{
    [JsonPropertyName("objects")]
    public List<CountryObjectDto>? Objects { get; init; }
}

public record CountryObjectDto
{
    [JsonPropertyName("names")]
    public CountryNamesDto? Names { get; init; }

    [JsonPropertyName("codes")]
    public CountryCodesDto? Codes { get; init; }
  
    [JsonPropertyName("currencies")]
    public List<CurrencyDto>? Currencies { get; init; }
}

public record CountryNamesDto
{
    [JsonPropertyName("common")]
    public string? Common { get; init; }
}

public record CountryCodesDto
{
    [JsonPropertyName("ccn3")]
    public string? Ccn3 { get; init; }
}

public record CurrencyDto
{
    [JsonPropertyName("code")]
    public string? Code { get; init; }
}