using Microsoft.Extensions.Options;
using WeatherWatch.Application.Cities;

namespace WeatherWatch.Infrastructure.Cities.Dtos;

public class RestCountriesClient(HttpClient httpClient, IOptions<RestCountriesOptions> options) : ICityService
{
    private readonly RestCountriesOptions _options = options.Value;

    public async Task<CreateCityResult> CreateCity(CreateCityRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}