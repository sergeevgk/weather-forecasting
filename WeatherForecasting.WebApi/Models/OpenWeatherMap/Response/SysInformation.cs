using Newtonsoft.Json;

namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;

public class SysInformation
{
	[JsonProperty("type")]
	public int Type { get; set; }

	[JsonProperty("id")]
	public int Id { get; set; }

	[JsonProperty("country")]
	public string Country { get; set; }

	[JsonProperty("sunrise")]
	public long Sunrise { get; set; }

	[JsonProperty("sunset")]
	public long Sunset { get; set; }
}
