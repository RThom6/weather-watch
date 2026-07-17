using WeatherWatch.Application.Weather;

namespace WeatherWatch.Api.Endpoints;

public static class WeatherEndpoints
{
    public static IEndpointRouteBuilder MapWeatherEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/weather/current", async (
                double latitude,
                double longitude,
                IWeatherService weatherService,
                CancellationToken cancellationToken) =>
            {
                var forecast = await weatherService.GetCurrentWeatherByCoordinates(latitude, longitude, cancellationToken: cancellationToken);
                return Results.Ok(forecast);
            })
            .WithName("GetWeatherForecast");
        
        return app;
    }
}