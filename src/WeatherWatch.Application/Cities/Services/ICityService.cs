namespace WeatherWatch.Application.Cities.Services;

public interface ICityService
{
    public Task<CreateCityResult> CreateCity(CreateCityRequest request, CancellationToken cancellationToken = default);
    public Task<CityDetailsPreview?> GetCityDetailsPreview(int cityId, CancellationToken cancellationToken = default);
    public Task<CityDetails> GetCityDetails(int cityId, CancellationToken cancellationToken = default);
    public Task<FindCitiesResult> FindCitiesByName(string name, CancellationToken cancellationToken = default);
    public Task<IReadOnlyList<CitySummary>> SearchCities(string name, int skip = 0, int take = 50, CancellationToken cancellationToken = default);
    public Task<UpdateCityResult> UpdateCity(int cityId, UpdateCityRequest request, CancellationToken cancellationToken = default);
    public Task<DeleteCityResult> DeleteCity(int cityId, CancellationToken cancellationToken = default);
}