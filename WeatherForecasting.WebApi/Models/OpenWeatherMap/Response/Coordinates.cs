using Newtonsoft.Json;

namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;


public struct Coordinates
{
	[JsonProperty("lon")]
	public decimal Lon { get; set; }

	[JsonProperty("lat")]
	public decimal Lat { get; set; }
}
