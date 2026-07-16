namespace WeatherWatch.Application.Weather;

public record Temp
{
    public decimal Celsius { get; init; }
    public decimal Fahrenheit => Celsius * 9 / 5 + 32;
}