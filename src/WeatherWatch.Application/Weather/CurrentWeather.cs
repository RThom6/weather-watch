namespace WeatherWatch.Application.Weather;

public record CurrentWeather
{
    public required string Summary { get; init; }
    public required string Condition { get; init; }
    public double TemperatureCelsius { get; init; }
    public double FeelsLikeCelsius { get; init; }
    public int Humidity { get; init; }
    public double WindSpeed { get; init; }
    public DateTimeOffset ObservedAt { get; init; }
}
