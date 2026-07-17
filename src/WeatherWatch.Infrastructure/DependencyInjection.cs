using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherWatch.Application.Cities;
using WeatherWatch.Application.Cities.Services;
using WeatherWatch.Application.Weather;
using WeatherWatch.Infrastructure.Cities;
using WeatherWatch.Infrastructure.Cities.Dtos;
using WeatherWatch.Infrastructure.Weather;

namespace WeatherWatch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Options
        var weatherOptions
            = configuration.GetSection("OpenWeather").Get<OpenWeatherOptions>() ??
              throw new InvalidOperationException("OpenWeather config missing");
        
        var restCountriesOptions
            = configuration.GetSection("RestCountries").Get<RestCountriesOptions>() ??
              throw new InvalidOperationException("RestCountries config missing");

        // HttpClients
        services.AddHttpClient<IWeatherService, OpenWeatherClient>(client =>
        {
            client.BaseAddress = new Uri(weatherOptions.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(5);
        });

        services.AddHttpClient<ICountryLookupClient, RestCountriesClient>(client =>
        {
            client.BaseAddress = new Uri(restCountriesOptions.BaseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", restCountriesOptions.ApiKey);
            client.Timeout = TimeSpan.FromSeconds(5);
        });
        
        // Options Configuration
        services.Configure<OpenWeatherOptions>(configuration.GetSection("OpenWeather"));
        
        // DbContext
        services.AddDbContext<CityDbContext>(opt =>
            opt.UseSqlite(configuration.GetConnectionString("CityStore")));
        
        services.AddScoped<ICityService, CityService>();
        
        return services;
    }
}
