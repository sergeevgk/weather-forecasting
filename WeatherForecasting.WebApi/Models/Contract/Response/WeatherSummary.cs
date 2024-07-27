namespace WeatherForecasting.WebApi.Models.Contract.Response;

using System.Text.Json.Serialization;

public class WeatherSummary
{
	[JsonPropertyName("main")]
	public string Main { get; set; }

	[JsonPropertyName("description")]
	public string Description { get; set; }

	[JsonPropertyName("temperature")]
	public Temperature Temperature { get; set; }

	[JsonPropertyName("pressure")]
	public int Pressure { get; set; }

	[JsonPropertyName("humidity")]
	public int Humidity { get; set; }

	[JsonPropertyName("cloudy_percentage")]
	public int CloudyPercentage { get; set; }

	[JsonPropertyName("rain_volume_3h")]
	public double RainVolume3Hour { get; set; }

	[JsonPropertyName("rain_volume_1h")]
	public double RainVolume1Hour { get; set; }

	[JsonPropertyName("snow_volume_3h")]
	public double SnowVolume3Hour { get; set; }

	[JsonPropertyName("snow_volume_1h")]
	public double SnowVolume1Hour { get; set; }

}
