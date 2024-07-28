using Newtonsoft.Json;

namespace WeatherForecasting.WebApi.Models.Contract.Response;

public class WeatherForecastResponse
{
	[JsonProperty("place")]
	public Place Place { get; set; }

	[JsonProperty("forecasts")]
	public WeatherForecastItem[] Forecasts { get; set; }
}
