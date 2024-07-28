using Newtonsoft.Json;

namespace WeatherForecasting.WebApi.Models.Contract.Response;

public class WeatherSummary
{
	[JsonProperty("main")]
	public string Main { get; set; }

	[JsonProperty("description")]
	public string Description { get; set; }

	[JsonProperty("temperature")]
	public Temperature Temperature { get; set; }

	[JsonProperty("pressure")]
	public int Pressure { get; set; }

	[JsonProperty("humidity")]
	public int Humidity { get; set; }

	[JsonProperty("cloudy_percentage")]
	public int CloudyPercentage { get; set; }

	[JsonProperty("rain_volume_3h")]
	public double RainVolume3Hour { get; set; }

	[JsonProperty("rain_volume_1h")]
	public double RainVolume1Hour { get; set; }

	[JsonProperty("snow_volume_3h")]
	public double SnowVolume3Hour { get; set; }

	[JsonProperty("snow_volume_1h")]
	public double SnowVolume1Hour { get; set; }

}
