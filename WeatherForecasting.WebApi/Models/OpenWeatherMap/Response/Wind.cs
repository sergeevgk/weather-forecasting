using Newtonsoft.Json;

namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;

public class Wind
{
	[JsonProperty("speed")]
	public double Speed { get; set; }

	[JsonProperty("deg")]
	public int Deg { get; set; }

	[JsonProperty("gust")]
	public double Gust { get; set; }
}
