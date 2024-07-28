using Newtonsoft.Json;

namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;


public class Clouds
{
	[JsonProperty("all")]
	public int All { get; set; }
}
