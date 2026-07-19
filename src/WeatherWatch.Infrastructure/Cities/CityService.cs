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
        var countryInfo = await countryLookupClient.GetCountryByIsoCode(request.CountryCode, cancellationToken);
        
        var requestedCity = countryInfo.Capitals.FirstOrDefault(c => c.Name == request.Name);

        if (requestedCity is null)
        {
            return new CreateCityResult
            {
                IsSuccess = false,
                ErrorMessage = $"City {request.Name} was not found"
            };
        }

        var city
            = new City
            {
                Name = request.Name,
                Country = countryInfo.Name,
                CountryCode = countryInfo.IsoCode,
                CurrencyCode = countryInfo.CurrencyCode,
                Latitude = requestedCity.Latitude,
                Longitude = requestedCity.Longitude,
                Timezone = countryInfo.Timezone
            };
        
        dbContext.Cities.Add(city);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateCityResult { IsSuccess = true, CityId = city.CityId };
    }

    public async Task<FindCitiesResult> FindCitiesByName(string name,
        CancellationToken cancellationToken = default)
    {
        var countries = await countryLookupClient.GetCitiesByName(name, cancellationToken);
        
        return new FindCitiesResult
        {
            Cities = countries
                .SelectMany(country => country.Capitals
                    .Select(capital => new SimpleCityInfo
                    {
                        Name = capital.Name,
                        Country = country.Name,
                        CountryCode = country.IsoCode,
                    }))
                .ToList()
        };
    }

    public async Task<CityDetails> GetCityDetails(int cityId, CancellationToken cancellationToken = default)
    {
        var city
            = await dbContext.Cities
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CityId == cityId, cancellationToken);

        if (city is null)
            return null;

        var forecasts
            = await weatherService.GetFiveDayForecastByCoordinates(
                city.Latitude,
                city.Longitude,
                cancellationToken: cancellationToken);

        var currentWeather
            = await weatherService.GetCurrentWeatherByCoordinates(
                city.Latitude,
                city.Longitude,
                cancellationToken: cancellationToken);

        return new CityDetails
        {
            CityId = city.CityId,
            Name = city.Name,
            Country = city.Country,
            CountryCode = city.CountryCode,
            CurrencyCode = city.CurrencyCode,
            EstimatedPopulation = city.EstimatedPopulation,
            TouristRating = city.TouristRating,
            DateEstablished = city.DateEstablished,
            Forecast = forecasts,
            CurrentWeather = currentWeather,
            UtcOffsetSeconds = ParseUtcOffsetSeconds(city.Timezone)
        };
    }

    // Converts a RestCountries timezone to shift in seconds
    private static int ParseUtcOffsetSeconds(string? timezone)
    {
        if (string.IsNullOrWhiteSpace(timezone))
            return 0;

        var text = timezone.Replace("UTC", "").Trim();
        if (text.Length == 0)
            return 0;

        var sign = text[0] == '-' ? -1 : 1;
        var magnitude = text.TrimStart('+', '-');

        return TimeSpan.TryParse(magnitude, out var offset) ? sign * (int)offset.TotalSeconds : 0;
    }

    public async Task<CityDetailsPreview?> GetCityDetailsPreview(int cityId, CancellationToken cancellationToken = default)
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

        return new CityDetailsPreview
        {
            CityId = city.CityId,
            Name = city.Name,
            Country = city.Country,
            CurrentWeather = currentWeather,
            UtcOffsetSeconds = ParseUtcOffsetSeconds(city.Timezone)
        };
    }
    
    public async Task<IReadOnlyList<CitySummary>> SearchCities(string name, int skip = 0, int take = 50, CancellationToken cancellationToken = default)
    {
        // check whether search is an id or a name
        if (int.TryParse(name, out int cityId))
        {
            return await dbContext.Cities
                .AsNoTracking()
                .Where(c => c.CityId == cityId)
                .OrderBy(c => c.Name)
                .Skip(skip)
                .Take(take)
                .Select(c => new CitySummary { CityId = c.CityId, Name = c.Name, CountryName = c.Country})
                .ToListAsync(cancellationToken);
        }
      
        return await dbContext.Cities
            .AsNoTracking()
            .Where(c => EF.Functions.Like(c.Name, $"%{name}%"))
            .OrderBy(c => c.Name)
            .Skip(skip)
            .Take(take)
            .Select(c => new CitySummary { CityId = c.CityId, Name = c.Name, CountryName = c.Country})
            .ToListAsync(cancellationToken);
    }

    public async Task<UpdateCityResult> UpdateCity(
        int cityId,
        UpdateCityRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request is { DateEstablished: null, EstimatedPopulation: null, TouristRating: null })
        {
            return new UpdateCityResult
            {
                IsSuccess = false,
                ErrorMessage = $"Failed to update city {cityId}. Rating, Date, and Population were all empty"
            };
        }
        
        var city
            = await dbContext.Cities
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CityId == cityId, cancellationToken);

        if (city is null)
        {
            return new UpdateCityResult
            {
                IsSuccess = false,
                ErrorMessage = $"City {cityId} not found"
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

    public async Task<DeleteCityResult> DeleteCity(int cityId, CancellationToken cancellationToken = default)
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