using Newtonsoft.Json;

namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;

public struct GeocodingResponse
{
	[JsonProperty("name")]
	public string Name { get; set; }
	[JsonProperty("country")]
	public string Country { get; set; }
	[JsonProperty("state")]
	public string State { get; set; }
	[JsonProperty("local_names")]
	public Dictionary<string, string> LocalNames { get; set; }

	[JsonProperty("lon")]
	public decimal Lon { get; set; }

	[JsonProperty("lat")]
	public decimal Lat { get; set; }
}
