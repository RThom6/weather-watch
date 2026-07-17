using WeatherWatch.Application.Cities;
using WeatherWatch.Application.Cities.Services;

namespace WeatherWatch.Infrastructure.Cities;

public class CityService(
    ICountryLookupClient countryLookupClient,
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

    public async Task<CityDetails> GetCityDetails(Guid cityId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}