using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherWatch.Application.Weather;
using WeatherWatch.Infrastructure.Weather;

namespace WeatherWatch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var options
            = configuration.GetSection("OpenWeather").Get<OpenWeatherOptions>() ??
              throw new InvalidOperationException("OpenWeather config missing");

        services.AddHttpClient<IWeatherService, OpenWeatherClient>(client =>
        {
            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(5);
        });
        
        services.Configure<OpenWeatherOptions>(configuration.GetSection("OpenWeather"));
        
        return services;
    }
}
