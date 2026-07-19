using System.Text.Json.Serialization;

namespace WeatherWatch.Infrastructure.Cities.Dtos;

// Relevant info from https://restcountries.com/docs/countries api response
// Can extend to capture more of the return info
internal sealed record CountryResponseDto
{
    [JsonPropertyName("data")]
    public CountryDataDto? Data { get; init; }
}
  
internal sealed record CountryDataDto
{
    [JsonPropertyName("objects")]
    public IReadOnlyList<CountryObjectDto>? Objects { get; init; } = [];
}

internal sealed record CountryObjectDto
{
    [JsonPropertyName("names")]
    public CountryNamesDto? Names { get; init; }

    [JsonPropertyName("codes")]
    public CountryCodesDto? Codes { get; init; }
  
    [JsonPropertyName("currencies")]
    public IReadOnlyList<CurrencyDto>? Currencies { get; init; } = [];

    [JsonPropertyName("timezones")]
    public IReadOnlyList<string>? Timezones { get; init; } = [];

    [JsonPropertyName("capitals")]
    public IReadOnlyList<CapitalDto> Capitals { get; init; } = [];
}

internal sealed record CountryNamesDto
{
    [JsonPropertyName("common")]
    public string? Common { get; init; }
}

internal sealed record CapitalDto
{
    [JsonPropertyName("coordinates")]
    public CoordinateDto? Coordinates { get; init; }
    
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

internal sealed record CoordinateDto
{
    [JsonPropertyName("lat")]
    public double Latitude { get; init; }
    [JsonPropertyName("lng")]
    public double Longitude { get; init; }
}

internal sealed record CountryCodesDto
{
    [JsonPropertyName("ccn3")]
    public string? Ccn3 { get; init; }
}

internal sealed record CurrencyDto
{
    [JsonPropertyName("code")]
    public string? Code { get; init; }
}