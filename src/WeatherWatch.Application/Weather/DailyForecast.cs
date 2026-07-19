namespace WeatherWatch.Application.Weather;

public record DailyForecast
{
    public required string Summary { get; init; }
    public required string Condition { get; init; }
    public int WeatherId { get; init; }
    public string? Icon { get; init; }
    public DateTimeOffset ForecastedAt { get; init; }
    public required DailyTemperature Temperature { get; init; }
    public required DailyTemperature FeelsLike { get; init; }
    public int PressureHpa { get; init; }
    public int Humidity { get; init; }
    public double WindSpeed { get; init; }
    public int WindDirectionDegrees { get; init; }
    public double? WindGust { get; init; }
    public int Cloudiness { get; init; }
    public double? RainMm { get; init; }
    public double? SnowMm { get; init; }
    public double PrecipitationChance { get; init; }
}

public record DailyTemperature
{
    public double Day { get; init; }
    public double Night { get; init; }
    public double Eve { get; init; }
    public double Morn { get; init; }
    public double? Min { get; init; }
    public double? Max { get; init; }
}
