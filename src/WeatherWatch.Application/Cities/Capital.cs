namespace WeatherWatch.Application.Cities;

public record Capital
{
    public string Name { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}