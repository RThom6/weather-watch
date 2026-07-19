namespace WeatherWatch.Application.Weather;

public record DailyForecast
{
    public required string Summary { get; init; }
    public required string Condition { get; init; }
    public string? Icon { get; init; }
    public DateOnly Date { get; init; }
    public double MinCelsius { get; init; }
    public double MaxCelsius { get; init; }
    public double DayCelsius { get; init; }
    public int Humidity { get; init; }
    public double WindSpeed { get; init; }
    public double PrecipitationChance { get; init; }
}
