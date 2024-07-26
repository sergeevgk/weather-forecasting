namespace WeatherForecasting.WebApi.Models.Response;

using System.Text.Json.Serialization;

public class Coordinates
{
	[JsonPropertyName("lon")]
	public decimal Lon { get; set; }

	[JsonPropertyName("lat")]
	public decimal Lat { get; set; }
}
