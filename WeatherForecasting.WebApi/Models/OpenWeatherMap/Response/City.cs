using Newtonsoft.Json;

namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;

public class City
{
	[JsonProperty("id")]
	public int Id { get; set; }

	[JsonProperty("name")]
	public string Name { get; set; }

	[JsonProperty("coord")]
	public Coordinates Coordinates { get; set; }

	[JsonProperty("country")]
	public string Country { get; set; }

	[JsonProperty("population")]
	public int Population { get; set; }

	[JsonProperty("timezone")]
	public int Timezone { get; set; }

	[JsonProperty("sunrise")]
	public long Sunrise { get; set; }

	[JsonProperty("sunset")]
	public long Sunset { get; set; }
}
