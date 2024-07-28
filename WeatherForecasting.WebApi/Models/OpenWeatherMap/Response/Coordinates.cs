using Newtonsoft.Json;

namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;


public class Coordinates
{
	[JsonProperty("lon")]
	public decimal Lon { get; set; }

	[JsonProperty("lat")]
	public decimal Lat { get; set; }
}
