using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using WeatherWatch.Application.Cities;
using WeatherWatch.Application.Cities.Services;
using WeatherWatch.Infrastructure.Cities.Dtos;

namespace WeatherWatch.Infrastructure.Cities;

public class RestCountriesClient(HttpClient httpClient) : ICountryLookupClient
{
    public async Task<IReadOnlyList<Country>> GetCitiesByName(string name,
        CancellationToken cancellationToken = default)
    {
        var uri = $"countries/v5/capitals?q={name}";
        
        var response 
            = await httpClient.GetFromJsonAsync<CountryResponseDto>(uri, cancellationToken) 
            ?? throw new InvalidOperationException("RestCountries returned an empty result");

        var countries = response.Data?.Objects;

        return countries is null ? [] : countries.Select(ToCountry).ToList();
    }
    
    public async Task<Country> GetCountryByIsoCode(string isoCode, CancellationToken cancellationToken = default)
    {
        var uri = $"countries/v5/code?q={isoCode}";
        
        var response 
            = await httpClient.GetFromJsonAsync<CountryResponseDto>(uri, cancellationToken) 
              ?? throw new InvalidOperationException("RestCountries returned an empty result");

        var country = response.Data?.Objects?.FirstOrDefault()
                      ?? throw new InvalidOperationException($"No country found for code '{isoCode}'");
        
        return ToCountry(country);
    }
    
    private Country ToCountry(CountryObjectDto country)
    {
        return new Country
        {
            Name = country.Names?.Common ?? "",
            IsoCode = country.Codes?.Ccn3 ?? "",
            CurrencyCode = country.Currencies?.FirstOrDefault()?.Code,
            Capitals = country.Capitals.Select(c => new Capital
            {
                Name = c.Name,
                Latitude = c.Coordinates.Latitude,
                Longitude = c.Coordinates.Longitude
            }).ToList()
        };
    }
}