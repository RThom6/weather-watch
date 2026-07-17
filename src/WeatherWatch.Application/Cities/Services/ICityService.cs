namespace WeatherWatch.Application.Cities.Services;

public interface ICityService
{
    public Task<CreateCityResult> CreateCity(CreateCityRequest request, CancellationToken cancellationToken = default);
    // GetCityDetails
    // UpdateCity
    // DeleteCity
    // SearchCity
}