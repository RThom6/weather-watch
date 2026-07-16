namespace WeatherWatch.Application.Weather;

public record DailyForecast
{
    public Temp Temperature { get; init; }
    public DateOnly Date { get; set; }
    public string? Summary { get; set; }
}