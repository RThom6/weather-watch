using Microsoft.EntityFrameworkCore;
using WeatherWatch.Application.Cities;
using WeatherWatch.Application.Cities.Services;
using WeatherWatch.Application.Weather;

namespace WeatherWatch.Infrastructure.Cities;

public class CityService(
    ICountryLookupClient countryLookupClient,
    IWeatherService weatherService,
    CityDbContext dbContext
    ) : ICityService
{
    public async Task<CreateCityResult> CreateCity(CreateCityRequest request,
        CancellationToken cancellationToken = default)
    {
        // Validation
        var countryInfo = await countryLookupClient.GetCountryByIsoCode(request.CountryCode, cancellationToken);

        var city
            = new City
            {
                CityId = Guid.NewGuid(),
                Name = request.Name,
                State = request.State,
                Country = countryInfo.Name,
                CountryCode = countryInfo.IsoCode,
                CurrencyCode = countryInfo.CurrencyCode
            };
        
        dbContext.Cities.Add(city);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateCityResult { IsSuccess = true, CityId = city.CityId };
    }

    public async Task<CityDetails?> GetCityDetails(Guid cityId, CancellationToken cancellationToken = default)
    {
        var city
            = await dbContext.Cities
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CityId == cityId, cancellationToken);

        if (city is null)
            return null;

        var currentWeather
            = await weatherService.GetCurrentWeatherByCoordinates(city.Latitude, city.Longitude,
                cancellationToken: cancellationToken);

        return new CityDetails
        {
            CityId = city.CityId,
            Name = city.Name,
            State = city.State,
            Country = city.Country,
            CountryCode = city.CountryCode,
            CurrencyCode = city.CurrencyCode,
            CurrentWeather = currentWeather
        };
    }
    
    public async Task<IReadOnlyList<CitySummary>> SearchCities(string name, CancellationToken cancellationToken = default)
    {
        return await dbContext.Cities
            .AsNoTracking()
            .Where(c => EF.Functions.Like(c.Name, $"%{name}%"))
            .Select(c => new CitySummary { CityId = c.CityId, Name = c.Name, CountryName = c.Country})
            .ToListAsync(cancellationToken);
    }
}