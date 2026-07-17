using Microsoft.EntityFrameworkCore;
using WeatherWatch.Application.Cities;

namespace WeatherWatch.Infrastructure.Cities;

public class CityDbContext(DbContextOptions<CityDbContext> options) : DbContext(options)
{
    public DbSet<City> Cities => Set<City>();
}