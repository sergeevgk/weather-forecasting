using Newtonsoft.Json;

namespace WeatherForecasting.WebApi.Models.Contract.Response;

public struct Temperature
{
	[JsonProperty("value")]
	public double Value { get; set; }

	[JsonProperty("feels_like")]
	public double FeelsLike { get; set; }
}
