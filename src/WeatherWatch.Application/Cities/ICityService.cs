namespace WeatherWatch.Application.Cities;

public interface ICityService
{
    public Task<CreateCityResult> CreateCity(CreateCityRequest request, CancellationToken cancellationToken = default);
    // GetCityDetails
    // UpdateCity
    // DeleteCity
    // SearchCity
}