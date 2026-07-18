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
                ICityService cityService,
                CancellationToken cancellationToken) =>
            {
                var request = new CreateCityRequest
                {
                    Name = name,
                    State = state,
                    Country = country,
                    CountryCode = countryCode,
                };
                
                var result = await cityService.CreateCity(request, cancellationToken);
                return Results.Ok(result);
            })
            .WithName("CreateCity");
        
        app.MapGet("/cities/{cityId}/details", async (
            Guid cityId,
            ICityService cityService,
            CancellationToken cancellationToken
        ) =>
        {
            var details = await cityService.GetCityDetails(cityId, cancellationToken);
            return details is not null ? Results.Ok(details) : Results.NotFound();
        })
        .WithName("GetCityDetails");
        
        return app;
    }
}