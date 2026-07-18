using WeatherWatch.Api.Endpoints;
using WeatherWatch.Infrastructure;
using WeatherWatch.Infrastructure.Cities;

namespace WeatherWatch.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddInfrastructure(builder.Configuration);
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Frontend", policy =>
                policy.WithOrigins("http://localhost:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        var app = builder.Build();
        
        using (var scope = app.Services.CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<CityDbContext>().Database.EnsureCreated();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapWeatherEndpoints();
        app.MapCityEndpoints();

        app.Run();
    }
}