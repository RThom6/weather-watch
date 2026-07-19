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
        
        app.MapGet("/cities/find", async (
            string name,
            ICityService cityService,
            CancellationToken cancellationToken
        ) =>
        {
            var result = await cityService.FindCitiesByName(name, cancellationToken);
            return Results.Ok(result);
        })
        .WithName("FindCities");
        
        app.MapGet("/cities/{cityId}/details/preview", async (
            int cityId,
            ICityService cityService,
            CancellationToken cancellationToken
        ) =>
        {
            var details = await cityService.GetCityDetailsPreview(cityId, cancellationToken);
            return details is not null ? Results.Ok(details) : Results.NotFound();
        })
        .WithName("GetCityDetails");
        
        app.MapGet("/cities/search", async (
                string name,
                ICityService cityService,
                CancellationToken cancellationToken,
                int page = 0,
                int pageSize = 50
                ) =>
            {
                page = Math.Max(0, page);
                pageSize = Math.Clamp(pageSize, 1, 50); // maximum page size

                var skip = page * pageSize;
                var results = await cityService.SearchCities(name, skip, pageSize, cancellationToken);
                return Results.Ok(results);
            })
            .WithName("SearchCities");
        
        app.MapPatch("/cities/{cityId}/update", async (
            int cityId,
            UpdateCityRequest request,
            ICityService cityService,
            CancellationToken cancellationToken) =>
            {
                var result = await cityService.UpdateCity(cityId, request, cancellationToken);

                return Results.Ok(result);
            })
        .WithName("UpdateCity");
        
        app.MapDelete("/cities/{cityId}", async (
            int cityId,
            ICityService cityService,
            CancellationToken cancellationToken) =>
                Results.Ok((object?)await cityService.DeleteCity(cityId, cancellationToken)))
            .WithName("DeleteCity");
        
        return app;
    }
}