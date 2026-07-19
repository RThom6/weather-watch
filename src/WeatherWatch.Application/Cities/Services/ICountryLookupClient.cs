namespace WeatherWatch.Application.Cities.Services;

public interface ICountryLookupClient
{
    public Task<IReadOnlyList<Country>> GetCitiesByName(string name,
        CancellationToken cancellationToken = default);
    public Task<Country> GetCountryByIsoCode(string countryCode, CancellationToken cancellationToken = default);
}