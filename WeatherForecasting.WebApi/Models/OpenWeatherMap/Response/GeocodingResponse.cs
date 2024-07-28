using Newtonsoft.Json;

namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;

public class GeocodingResponse : Coordinates
{
	[JsonProperty("name")]
	public string Name { get; set; }
	[JsonProperty("country")]
	public string Country { get; set; }
	[JsonProperty("state")]
	public string State { get; set; }
	[JsonProperty("local_names")]
	public Dictionary<string, string> LocalNames { get; set; }
}
