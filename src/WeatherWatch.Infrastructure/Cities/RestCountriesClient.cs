using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using WeatherWatch.Application.Cities;
using WeatherWatch.Application.Cities.Services;
using WeatherWatch.Infrastructure.Cities.Dtos;

namespace WeatherWatch.Infrastructure.Cities;

public class RestCountriesClient(HttpClient httpClient) : ICountryLookupClient
{
    public async Task<Country> GetCountryByIsoCode(string isoCode, CancellationToken cancellationToken = default)
    {
        var uri = $"countries/v5/code?q={isoCode}";
        
        var response 
            = await httpClient.GetFromJsonAsync<CountryResponseDto>(uri, cancellationToken) 
              ?? throw new InvalidOperationException("RestCountries returned an empty result");

        var country = response.Data?.Objects?.FirstOrDefault()
                      ?? throw new InvalidOperationException($"No country found for code '{isoCode}'");
        
        return new Country
        {
            Name = country.Names?.Common ?? "",
            IsoCode = country.Codes?.Ccn3 ?? "",
            CurrencyCode = country.Currencies?.FirstOrDefault()?.Code,
        };
    }
}