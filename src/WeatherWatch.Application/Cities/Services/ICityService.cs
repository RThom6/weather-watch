namespace WeatherWatch.Application.Cities.Services;

public interface ICityService
{
    public Task<CreateCityResult> CreateCity(CreateCityRequest request, CancellationToken cancellationToken = default);
    public Task<CityDetails?> GetCityDetails(Guid cityId, CancellationToken cancellationToken = default);
    public Task<IReadOnlyList<CitySummary>> SearchCities(string name, CancellationToken cancellationToken = default);
    // UpdateCity
    // DeleteCity
}