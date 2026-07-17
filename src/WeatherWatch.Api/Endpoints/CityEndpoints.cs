using WeatherWatch.Application.Cities;
using WeatherWatch.Application.Cities.Services;

namespace WeatherWatch.Api.Endpoints;

public static class CityEndpoints
{
    public static IEndpointRouteBuilder MapCityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/cities/create", async (
                string name,
                string state,
                string country,
                string countryCode,
                int? touristRating,
                DateOnly? dateEstablished,
                int? estimatedPopulation,
                ICityService cityService,
                CancellationToken cancellationToken) =>
            {
                var request = new CreateCityRequest
                {
                    Name = name,
                    State = state,
                    Country = country,
                    CountryCode = countryCode,
                    TouristRating = touristRating,
                    DateEstablished = dateEstablished,
                    EstimatedPopulation = estimatedPopulation
                };
                
                var result = await cityService.CreateCity(request, cancellationToken);
                return Results.Ok(result);
            })
            .WithName("CreateCity");
        
        return app;
    }
}