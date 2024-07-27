namespace WeatherForecasting.WebApi.Models.Contract.Response;

using System.Text.Json.Serialization;

public class Temperature
{
	[JsonPropertyName("value")]
	public double Value { get; set; }

	[JsonPropertyName("feels_like")]
	public double FeelsLike { get; set; }
}
