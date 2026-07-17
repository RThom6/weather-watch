namespace WeatherWatch.Application.Cities.Services;

public interface ICountryLookupClient
{
    public Task<Country> GetCountryByIsoCode(string countryCode, CancellationToken cancellationToken = default);
}