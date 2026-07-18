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

    public async Task<UpdateCityResult> UpdateCity(UpdateCityRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is { DateEstablished: null, EstimatedPopulation: null, TouristRating: null })
        {
            return new UpdateCityResult
            {
                IsSuccess = false,
                ErrorMessage = $"Failed to update city {request.CityId}. Rating, Date, and Population were all empty"
            };
        }
        
        var city
            = await dbContext.Cities
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CityId == request.CityId, cancellationToken);

        if (city is null)
        {
            return new UpdateCityResult
            {
                IsSuccess = false,
                ErrorMessage = $"City {request.CityId} not found"
            };
        }
        
        // Only update the ones that aren't null
        var updatedCity
            = city with
            {
                DateEstablished = request.DateEstablished ?? city.DateEstablished,
                EstimatedPopulation = request.EstimatedPopulation ?? city.EstimatedPopulation,
                TouristRating = request.TouristRating ?? city.TouristRating
            };

        dbContext.Update(updatedCity);
        int rows = await dbContext.SaveChangesAsync(cancellationToken);

        if (rows < 1)
        {
            return new UpdateCityResult
            {
                IsSuccess = false,
                ErrorMessage = "Something went wrong. Unable to update city"
            };
        }

        return new UpdateCityResult
        {
            IsSuccess = true,
            CityId = updatedCity.CityId,
        };
    }

    public async Task<DeleteCityResult> DeleteCity(Guid cityId, CancellationToken cancellationToken = default)
    {
        var city
            = await dbContext.Cities
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CityId == cityId, cancellationToken);

        if (city is null)
        {
            return new DeleteCityResult
            {
                IsSuccess = false,
                ErrorMessage = $"City {cityId} not found"
            };
        }

        dbContext.Remove(city);
        var rows = await dbContext.SaveChangesAsync(cancellationToken);
        
        if (rows < 1)
        {
            return new DeleteCityResult
            {
                IsSuccess = false,
                ErrorMessage = "Something went wrong. Unable to delete city"
            };
        }

        return new DeleteCityResult
        {
            IsSuccess = true,
        };
    }
}