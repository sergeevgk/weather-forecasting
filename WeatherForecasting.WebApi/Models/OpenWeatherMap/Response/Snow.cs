using Newtonsoft.Json;

namespace WeatherForecasting.WebApi.Models.OpenWeatherMap.Response;

public class Snow
{
	[JsonProperty("3h")]
	public double VolumeThreeHour { get; set; }

	[JsonProperty("1h")]
	public double VolumeOneHour { get; set; }
}
