using System.Text.Json.Serialization;

namespace WeatherForecasting.WebApi.Models.Contract.Response;

public class WeatherForecastResponse
{
	[JsonPropertyName("place")]
	public Place Place { get; set; }

	[JsonPropertyName("forecasts")]
	public WeatherForecastItem[] Forecasts { get; set; }
}
