using WeatherWatch.Application.Cities;
using WeatherWatch.Application.Cities.Services;

namespace WeatherWatch.Api.Endpoints;

public static class CityEndpoints
{
    public static IEndpointRouteBuilder MapCityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/cities/create", async (
                CreateCityRequest request,
                ICityService cityService,
                CancellationToken cancellationToken) =>
            {
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
        
        app.MapGet("/cities/search", async (
                string name,
                ICityService cityService,
                CancellationToken cancellationToken) =>
            {   
                var results = await cityService.SearchCities(name, cancellationToken);
                return Results.Ok(results);
            })
            .WithName("SearchCities");
        
        app.MapPatch("/cities/{cityId}/update", async (
            Guid cityId,
            UpdateCityRequest request,
            ICityService cityService,
            CancellationToken cancellationToken) =>
            {
                var result = await cityService.UpdateCity(cityId, request, cancellationToken);

                return Results.Ok(result);
            })
        .WithName("UpdateCity");
        
        app.MapDelete("/cities/{cityId}", async (
            Guid cityId,
            ICityService cityService,
            CancellationToken cancellationToken) =>
                Results.Ok((object?)await cityService.DeleteCity(cityId, cancellationToken)))
            .WithName("DeleteCity");
        
        return app;
    }
}